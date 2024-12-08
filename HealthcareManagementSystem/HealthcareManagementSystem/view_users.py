import sqlite3

# Connect to the database
conn = sqlite3.connect("users.db")

# Create a cursor to interact with the database
cursor = conn.cursor()

# Get the list of tables
cursor.execute("SELECT name FROM sqlite_master WHERE type='table';")
tables = cursor.fetchall()
print("Tables:", tables)

# View data from the 'users' table (replace 'users' with the actual table name)
cursor.execute("SELECT * FROM users;")
rows = cursor.fetchall()
for row in rows:
    print(row)

# Close the connection
conn.close()
