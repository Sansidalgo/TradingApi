import React, { useState } from 'react';
import { makeApiRequest, generateSymbol, parseFullSymbol } from './helpers.js';
import { subscribeOnStream, unsubscribeFromStream } from './streaming.js';

const TradingViewChart = () => {
    const [lastBarsCache, setLastBarsCache] = useState(new Map());
    const [configurationData, setConfigurationData] = useState({
        supported_resolutions: ['1D', '1W', '1M'],
        exchanges: [
            {
                value: 'Bitfinex',
                name: 'Bitfinex',
                desc: 'Bitfinex',
            },
            {
                value: 'Kraken',
                name: 'Kraken',
                desc: 'Kraken bitcoin exchange',
            },
            {
                value: 'NSE',
                name: 'National Stock Exchange',
                desc: 'NSE',
            },
        ],
        symbols_types: [
            {
                name: 'crypto',
                value: 'crypto',
            },
        ],
    });

    const getAllSymbols = async () => {
        const data = await makeApiRequest('data/v3/all/exchanges');
        let allSymbols = [];

        for (const exchange of configurationData.exchanges) {
            const pairs = data.Data[exchange.value].pairs;

            for (const leftPairPart of Object.keys(pairs)) {
                const symbols = pairs[leftPairPart].map(rightPairPart => {
                    const symbol = generateSymbol(exchange.value, leftPairPart, rightPairPart);
                    return {
                        symbol: symbol.short,
                        full_name: symbol.full,
                        description: symbol.short,
                        exchange: exchange.value,
                        type: 'crypto',
                    };
                });
                allSymbols = [...allSymbols, ...symbols];
            }
        }

        // Add NSE and NIFTY 50
        allSymbols.push({
            symbol: 'NSE:NIFTY',
            full_name: 'NSE:NIFTY',
            description: 'NIFTY 50 Index',
            exchange: 'NSE',
            type: 'index',
        });

        return allSymbols;
    };

    const handleOnReady = (callback) => {
        console.log('[onReady]: Method call');
        setTimeout(() => callback(configurationData));
    };

    const handleSearchSymbols = async (
        userInput,
        exchange,
        symbolType,
        onResultReadyCallback
    ) => {
        console.log('[searchSymbols]: Method call');
        const symbols = await getAllSymbols();
        const newSymbols = symbols.filter(symbol => {
            const isExchangeValid = exchange === '' || symbol.exchange === exchange;
            const isFullSymbolContainsInput =
                symbol.full_name.toLowerCase().indexOf(userInput.toLowerCase()) !== -1;
            return isExchangeValid && isFullSymbolContainsInput;
        });
        onResultReadyCallback(newSymbols);
    };

    const handleResolveSymbol = async (
        symbolName,
        onSymbolResolvedCallback,
        onResolveErrorCallback,
        extension
    ) => {
        console.log('[resolveSymbol]: Method call', symbolName);

        // Check if the symbol is NIFTY 50
        if (symbolName === 'NSE:NIFTY') {
            const symbolInfo = {
                ticker: 'NSE:NIFTY',
                name: 'NIFTY 50 Index',
                description: 'NIFTY 50 Index',
                type: 'index',
                session: '24x7',
                timezone: 'Asia/Kolkata',
                exchange: 'NSE',
                minmov: 1,
                pricescale: 100,
                has_intraday: false,
                has_no_volume: true,
                has_weekly_and_monthly: false,
                supported_resolutions: configurationData.supported_resolutions,
                volume_precision: 2,
                data_status: 'streaming',
            };

            console.log('[resolveSymbol]: Symbol resolved', symbolName);
            onSymbolResolvedCallback(symbolInfo);
            return;
        }

        const symbols = await getAllSymbols();
        const symbolItem = symbols.find(({ full_name }) => full_name === symbolName);

        if (!symbolItem) {
            console.log('[resolveSymbol]: Cannot resolve symbol', symbolName);
            onResolveErrorCallback('cannot resolve symbol');
            return;
        }

        // Symbol information object
        const symbolInfo = {
            ticker: symbolItem.full_name,
            name: symbolItem.symbol,
            description: symbolItem.description,
            type: symbolItem.type,
            session: '24x7',
            timezone: 'Etc/UTC',
            exchange: symbolItem.exchange,
            minmov: 1,
            pricescale: 100,
            has_intraday: false,
            has_no_volume: true,
            has_weekly_and_monthly: false,
            supported_resolutions: configurationData.supported_resolutions,
            volume_precision: 2,
            data_status: 'streaming',
        };

        console.log('[resolveSymbol]: Symbol resolved', symbolName);
        onSymbolResolvedCallback(symbolInfo);
    };

    const handleGetBars = async (
        symbolInfo,
        resolution,
        periodParams,
        onHistoryCallback,
        onErrorCallback
    ) => {
        const { from, to, firstDataRequest } = periodParams;
        console.log('[getBars]: Method call', symbolInfo, resolution, from, to);
        const parsedSymbol = parseFullSymbol(symbolInfo.full_name);
        const urlParameters = {
            e: parsedSymbol.exchange,
            fsym: parsedSymbol.fromSymbol,
            tsym: parsedSymbol.toSymbol,
            toTs: to,
            limit: 2000,
        };
        const query = Object.keys(urlParameters)
            .map(name => `${name}=${encodeURIComponent(urlParameters[name])}`)
            .join('&');
        try {
            const data = await makeApiRequest(`data/histoday?${query}`);
            if (data.Response && data.Response === 'Error' || data.Data.length === 0) {
                // "noData" should be set if there is no data in the requested period
                onHistoryCallback([], {
                    noData: true,
                });
                return;
            }
            let bars = [];
            data.Data.forEach(bar => {
                if (bar.time >= from && bar.time < to) {
                    bars = [...bars, {
                        time: bar.time * 1000,
                        low: bar.low,
                        high: bar.high,
                        open: bar.open,
                        close: bar.close,
                    }];
                }
            });
            if (firstDataRequest) {
                setLastBarsCache(new Map([[symbolInfo.full_name, { ...bars[bars.length - 1] }]]));
            }
            console.log(`[getBars]: returned ${bars.length} bar(s)`);
            onHistoryCallback(bars, {
                noData: false,
            });
        } catch (error) {
            console.log('[getBars]: Get error', error);
            onErrorCallback(error);
        }
    };

    const handleSubscribeBars = (
        symbolInfo,
        resolution,
        onRealtimeCallback,
        subscriberUID,
        onResetCacheNeededCallback
    ) => {
        console.log('[subscribeBars]: Method call with subscriberUID:', subscriberUID);
        subscribeOnStream(
            symbolInfo,
            resolution,
            onRealtimeCallback,
            subscriberUID,
            onResetCacheNeededCallback,
            lastBarsCache.get(symbolInfo.full_name)
        );
    };

    const handleUnsubscribeBars = (subscriberUID) => {
        console.log('[unsubscribeBars]: Method call with subscriberUID:', subscriberUID);
        unsubscribeFromStream(subscriberUID);
    };

    return (
        // JSX structure for your TradingViewChart component
        <div>
            {/* Your TradingViewChart component JSX here */}
        </div>
    );
};

export default TradingViewChart;
