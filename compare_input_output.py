#pip install pandas-visual-analysis

import pandas as pd
from sqlalchemy import create_engine
from pandas_visual_analysis import VisualAnalysis

# Function to read data from Excel
def read_excel(file_path):
    df = pd.read_excel(file_path, usecols=['recordid', 'column1', 'column2', 'column3'])
    return df

# Function to run SQL query and get data into DataFrame
def run_sql_query(record_id):
    # Connect to your MySQL database (update the connection string accordingly)
    engine = create_engine('mysql://username:password@localhost/dbname')
    
    # Replace 'your_table' with your actual table name
    query = f"SELECT recordid, column1, column2, column3 FROM your_table WHERE recordid = {record_id}"
    
    # Execute the query and get data into DataFrame
    df = pd.read_sql(query, engine)
    
    return df

# Main function
def main():
    # Update with your actual Excel file path
    excel_file_path = 'path/to/your/excel/file.xlsx'
    
    # Read data from Excel into DataFrame
    input_dataframe = read_excel(excel_file_path)
    
    # Connect to your MySQL database (update the connection string accordingly)
    engine = create_engine('mysql://username:password@localhost/dbname')
    
    # Replace 'your_table' with your actual table name
    table_name = 'your_table'
    
    # Create a list to store results
    results = []
    
    # Iterate over each row in the DataFrame
    for index, row in input_dataframe.iterrows():
        record_id = row['recordid']
        
        # Run SQL query to get data into DataFrame
        sql_dataframe = run_sql_query(record_id)
        
        # Compare DataFrames
        result = row.equals(sql_dataframe)
        results.append(result)
        
        # Display the comparison result
        print(f"\nComparing records for recordid: {record_id}")
        if result:
            print("Pass - DataFrames match.")
        else:
            print("Fail - DataFrames do not match.")
    
    # Create an HTML report
    report_df = pd.DataFrame(results, columns=['Result'])
    VisualAnalysis(report_df).to_html("comparison_report.html")

if __name__ == "__main__":
    main()
