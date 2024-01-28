# db/utils.py
# db/models.py
from sqlalchemy import create_engine, MetaData, Column, String, Integer, Float
from sqlalchemy.ext.automap import automap_base
from sqlalchemy.orm import Session
from .models import Base, OptionsData
from datetime import datetime
import pytz
from sqlalchemy.orm.exc import NoResultFound

def generate_models(database_url, table_name):
    engine = create_engine(database_url)
    metadata = MetaData()
    metadata.reflect(bind=engine)

    Base.prepare()
    generated_model = Base.classes.get(table_name)

    return generated_model

def insert_options_data(session, pcr_oi, pcr_oi_change, put_oi, call_oi, put_oi_change, call_oi_change, PEvwap, CEVWAP):
    india_timezone = pytz.timezone('Asia/Kolkata')
    current_datetime = datetime.now()

    # Format the current date and time without seconds
    formatted_datetime = current_datetime.strftime('%Y-%m-%d %H:%M')
    new_option = OptionsData(
            PcrOI=pcr_oi, 
            PcrOIChange=pcr_oi_change, 
            PutOI=put_oi, 
            CallOI=call_oi, 
            PutOIChange=put_oi_change, 
            CallOIChange=call_oi_change, 
            PEVWAP=PEvwap,
            CEVWAP=CEVWAP,
            EntryDateTime=formatted_datetime,
        )
    session.add(new_option)
    session.commit()
    print("Data inserted successfully")
