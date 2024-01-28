from sqlalchemy import create_engine, MetaData
from sqlalchemy.ext.automap import automap_base
from sqlalchemy.orm import Session

# Replace 'sqlite:///your_database.db' with the connection string for your database
engine = create_engine('Server=P3NWPLSK12SQL-v12.shr.prod.phx3.secureserver.net;Database=algo;User ID=algo;Password=Siddu_1990@;TrustServerCertificate=True;')

# Reflect the database
metadata = MetaData()
metadata.reflect(bind=engine)

# Use automap to generate classes
Base = automap_base(metadata=metadata)
Base.prepare()

# Access the generated classes
User = Base.classes.your_table_name

# Example: Query the database using the generated model
session = Session(engine)
users = session.query(User).all()

for user in users:
    print(user.id, user.name)  # Adjust the attribute names based on your table structure
