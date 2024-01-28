using BLU.Dtos;
using System.Reflection.Metadata.Ecma335;

public class OptionContractAnalyzerRepository
{
    // ... (other methods and properties)

    static double CalculateDynamicMinOI(List<OptionContractResponseDto> contracts)
    {
        // Calculate moving average of Open Interest
        double averageOI = contracts.Average(contract => contract.OpenInterest);
        double dynamicMinOI = averageOI * 1.2; // Adjust multiplier based on your strategy
        return dynamicMinOI;
    }

    static double CalculateDynamicMinDTR(List<OptionContractResponseDto> contracts)
    {
        // Calculate moving average of Daily Trading Range
        double averageDTR = contracts.Average(contract => contract.DailyTradingRange);
        double dynamicMinDTR = averageDTR * 1.2; // Adjust multiplier based on your strategy
        return dynamicMinDTR;
    }

    static string GetFilteredContracts(List<OptionContractResponseDto> contracts, string optionType, double minOI, double minDTR)
    {
        var sortedContracts = contracts
           .Where(contract => contract.OptionType == optionType)
           .OrderByDescending(contract => contract.OpenInterest)
           .ThenByDescending(contract => contract.DailyTradingRange)
           .FirstOrDefault().Symbol;
        // Apply filters based on minimum Open Interest and Daily Trading Range for the specified option type
        return sortedContracts;
    }

    public static string GetOptionContract(List<OptionContractResponseDto> nifty50Options, string optionType)
    {
        // Set your dynamic thresholds for PE and CE based on your strategy
        double dynamicMinOI = CalculateDynamicMinOI(nifty50Options);
        double dynamicMinDTR = CalculateDynamicMinDTR(nifty50Options);

        // Select PE and CE contracts based on dynamic thresholds
        return GetFilteredContracts(nifty50Options, optionType, dynamicMinOI, dynamicMinDTR);

    }

    static void PrintContracts(List<OptionContractResponseDto> contracts)
    {
        foreach (var contract in contracts)
        {
            Console.WriteLine($"Symbol: {contract.Symbol}, Strike Price: {contract.StrikePrice}, Option Type: {contract.OptionType}");
        }
    }

    // ... (other methods and properties)
}
