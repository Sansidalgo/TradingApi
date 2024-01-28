import http.client
import json
from datetime import datetime
import pytz
class NseApiLogic:
    def __init__(self):
        pass  # You can add any initialization logic here

    def fetch_option_chain(self):
        try:
            
            conn = http.client.HTTPSConnection("www.nseindia.com")
            payload = ''
            headers = {
            'Cookie': 'ak_bmsc=591732982B65F00C1F4D83CC5E81ABB0~000000000000000000000000000000~YAAQQ3xBF5YRtPaMAQAA4W7USxaeoRqEZBbPzzDOGlWbQ8VJEgI6Skr+zX9fj+J/jD+k6jLD5u/BRcNRRa0Min2lZFBwBAKbISwKeJ71IPVgHhGKOVCpbYQU3GQXMCpHE5kqNdVKM6/keppDLmb7p9fCVRN5mZOgPEglD0UArCe3LY25/mtaVYKpDXM6Q3GONv1Rf/iW4kypBjTf0GRikyC4lta786h4A28v1sHmOq5pDYaIj4pkAM9jYlLmC0AH+d3frHCC1JMqXUwt3qOVqi/Rr5sxFlO51JioWZh6gswn0vo3LaCkWqw0zCXa8nZgJHJqXrZI9h5D1LX+A9v2x1dgf9eyipm2SwrlyOmkyQHoswXIlMv8Wbzk3VEK2A=='
            }
            conn.request("GET", "/api/option-chain-indices?symbol=NIFTY", payload, headers)
            res = conn.getresponse()
            
            data =json.loads(res.read().decode("utf-8"))
            print(data)
            return data
        except Exception as e:
            print(f"Error fetching option chain: {e}")
            return None

    def calculate_pcr_and_vwap(self, response):
        if response:
            india_timezone = pytz.timezone('Asia/Kolkata')
            current_date = datetime.now()
            filtered_data = [record for record in response["records"]["data"] if "PE" in record and "CE" in record and datetime.strptime(record["expiryDate"], "%d-%b-%Y") >= current_date]

            if not filtered_data:
                print("Error: No valid option contracts found with expiry date greater than or equal to the current date.")
                return None, None, None, None, None, None, None, None

            put_open_interest = 0
            call_open_interest = 0
            put_open_interest_change = 0
            call_open_interest_change = 0
            vwap_pe_numerator = 0
            total_volume_pe = 0
            vwap_ce_numerator = 0
            total_volume_ce = 0

            for option_data in filtered_data:
                pe_data = option_data["PE"]
                ce_data = option_data["CE"]

                # Use PE data for calculations
                put_open_interest += pe_data["openInterest"]
                put_open_interest_change += pe_data["changeinOpenInterest"]

                last_price_pe = pe_data["lastPrice"]
                volume_pe = pe_data["totalTradedVolume"]
                vwap_pe_numerator += last_price_pe * volume_pe
                total_volume_pe += volume_pe

                # Use CE data for calculations
                call_open_interest += ce_data["openInterest"]
                call_open_interest_change += ce_data["changeinOpenInterest"]

                last_price_ce = ce_data["lastPrice"]
                volume_ce = ce_data["totalTradedVolume"]
                vwap_ce_numerator += last_price_ce * volume_ce
                total_volume_ce += volume_ce

            # Calculate PCR using both Open Interest and Change in Open Interest
            total_put_open_interest = put_open_interest + put_open_interest_change
            total_call_open_interest = call_open_interest + call_open_interest_change

            # Avoid division by zero
            pcr_with_oi = total_put_open_interest / total_call_open_interest if total_call_open_interest != 0 else float('inf')
            pcr_with_oi_change = put_open_interest_change / call_open_interest_change if call_open_interest_change != 0 else float('inf')

            # Calculate overall VWAP for both PE and CE
            vwap_pe = vwap_pe_numerator / total_volume_pe if total_volume_pe != 0 else 0
            vwap_ce = vwap_ce_numerator / total_volume_ce if total_volume_ce != 0 else 0

            # Return all requested variables
            return pcr_with_oi, pcr_with_oi_change, total_put_open_interest, total_call_open_interest, put_open_interest_change, call_open_interest_change, vwap_pe, vwap_ce
        else:
            print("Error: Invalid response.")
            return None, None, None, None, None, None, None, None

# Example usage:
# nse_logic = NseApiLogic()
# option_chain_response = nse_logic.fetch_option_chain()
# pcr_with_oi, pcr_with_oi_change, total_put_open_interest, total_call_open_interest, put_open_interest_change, call_open_interest_change, vwap_pe, vwap_ce = nse_logic.calculate_pcr_and_vwap(option_chain_response)
