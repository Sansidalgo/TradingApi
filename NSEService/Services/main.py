import logging
from sqlalchemy import create_engine
from db.models import OptionsData, Base
from db.utils import insert_options_data
from db.config import database_url
from sqlalchemy.orm import Session
from logic.nse_api_logic import NseApiLogic

def main():
    # Configure logging
    logging.basicConfig(filename='main.log', level=logging.ERROR, format='%(asctime)s - %(levelname)s - %(message)s', datefmt='%Y-%m-%d %H:%M:%S')

    try:
        nse_logic = NseApiLogic()

        # Fetch option chain data
        option_chain_response = nse_logic.fetch_option_chain()

        # Calculate PCR and VWAP
        result_tuple = nse_logic.calculate_pcr_and_vwap(option_chain_response)
        if None in result_tuple:
            logging.error("The result_tuple contains None. Returning from here.")
            return  # This will exit the function or method immediately

        engine = create_engine(database_url)
        # Base.metadata.create_all(engine)

        session = Session(engine)

        try:
            # Call the insert_options_data method with column values
            insert_options_data(session, *result_tuple)

            # Commit changes to the database
           
        except Exception as insert_error:
            logging.error(f"Error during database insertion: {insert_error}")
            # Optionally, you may want to rollback changes on error
            session.rollback()
        finally:
            # Remember to close the session when done
            session.close()

    except Exception as main_error:
        logging.error(f"Main function error: {main_error}")

if __name__ == "__main__":
    main()
