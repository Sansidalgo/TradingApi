# db/models.py
from sqlalchemy import create_engine, MetaData, Column, String, Integer, Float
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import Session

Base = declarative_base()

class OptionsData(Base):
    __tablename__ = 'tblOptionsData'
    PcrOI = Column(String, primary_key=True)
    PcrOIChange = Column(String)
    PutOI = Column(String)
    CallOI = Column(String)
    PutOIChange = Column(String)
    CallOIChange = Column(String)
    PEVWAP = Column(String)
    CEVWAP = Column(String)
    EntryDateTime=Column(String)
