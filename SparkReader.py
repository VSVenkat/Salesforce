from pyspark.sql import SparkSession

# Create a Spark session
spark = SparkSession.builder.appName("MySQLReadExample").getOrCreate()

# JDBC URL for the MySQL database
# Replace 'your_database_url', 'your_database_name', 'your_username', and 'your_password' with your actual MySQL credentials
jdbc_url = "jdbc:mysql://your_database_url:3306/your_database_name"
connection_properties = {
    "user": "your_username",
    "password": "your_password",
    "driver": "com.mysql.cj.jdbc.Driver"
}

# Specify the table you want to read
table_name = "your_table_name"

# Read data from MySQL into a PySpark DataFrame
df = spark.read.jdbc(url=jdbc_url, table=table_name, properties=connection_properties)

# Show the DataFrame
df.show()

# Stop the Spark session
spark.stop()
