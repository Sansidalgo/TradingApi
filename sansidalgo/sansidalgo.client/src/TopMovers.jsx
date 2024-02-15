import React, { useEffect, useState } from 'react';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChartGantt, faChartLine, faBolt, faBoltLightning } from '@fortawesome/free-solid-svg-icons';

function TopMovers() {

    const [apistatus, setApiStatus] = useState('');
    const [topGainersData, setTopGainersData] = useState([]);
    const [selectedTab, setSelectedTab] = useState(0);

   //getTopGainers();
    async function getTopGainers(){
        
        try {
            console.log("top gainers:");
            const url = "https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%2050";

            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36',
                    'Referrer': 'https://www.nseindia.com/',
                    'Accept': 'application/json',
                    // Add other headers if needed
                },
            });
            const data = await response.json();

            if (response.ok) {
                // index top gainers successful
                console.log('top gainers retrived successfully!');
                console.log(data);
                setApiStatus(data.message);
                setTopGainersData(data);
                
            } else {
                setApiStatus('Error: ' + data.message);
                console.error('Error:', data.message);
                throw new Error(data.message);
            }
        } catch (error) {
            setApiStatus('Error during pulling top gainers:', error);
            console.error('Error during pulling top gainers:', error);
        }

    }


  return (
      <section className="contact_section layout_padding-top">
          <div className="container-fluid">
              <div className="row">
                  <div className="col-lg-4 col-md-5 offset-md-1">
                      <div className="heading_container">
                          <h2>Top Movers</h2>
                      </div>
                  </div>
              </div>

              <div>
                  <Tabs id="subscriptionsTab" selectedIndex={selectedTab} onSelect={(index) => setSelectedTab(index)} >
                      <TabList>
                          <Tab>
                              <FontAwesomeIcon icon={faChartLine} /> Top Gainers
                          </Tab>
                          <Tab>
                              <FontAwesomeIcon icon={faChartGantt} /> Top Losers
                          </Tab>
                          <Tab>
                              <FontAwesomeIcon icon={faBolt} /> 52W High
                          </Tab>
                          <Tab>
                              <FontAwesomeIcon icon={faBoltLightning} /> 52W Low
                          </Tab>
                      </TabList>
                      <TabPanel>
                         <p>test</p>
                      </TabPanel>
                      <TabPanel>
                          <section className="subscription_section layout_padding-top">
                              <div className="row">
                              </div>
                          </section>
                      </TabPanel>
                      <TabPanel>
                          <section className="subscription_section layout_padding-top">
                              <div className="row">
                              </div>
                          </section>
                      </TabPanel>
                      <TabPanel>
                          <section className="subscription_section layout_padding-top">
                              <div className="row">
                              </div>
                          </section>
                      </TabPanel>
                  </Tabs>
              </div>
          </div>
      </section>
  );
}

export default TopMovers;