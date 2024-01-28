# Replace the connection details
import pyodbc 
import urllib
username = 'algo'
password = 'Siddu_1990@'
hostname = 'P3NWPLSK12SQL-v12.shr.prod.phx3.secureserver.net'
port = '1433'  # Replace with your actual port number
database_name = 'algo'

params = urllib.parse.quote_plus('DRIVER={ODBC Driver 17 for SQL Server};\
                      SERVER='+hostname+';\
                      DATABASE='+database_name+';\
                      UID='+username+';\
                      PWD='+ password)

database_url = "mssql+pyodbc:///?odbc_connect={}".format(params)

# Create the database URL
# Create the database URL with ODBC Driver 18
# database_url = f'mssql+pyodbc://{username}:{password}@{hostname}/{database_name}?driver=ODBC+Driver+17+for+SQL+Server'

