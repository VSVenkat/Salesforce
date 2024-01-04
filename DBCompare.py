import pandas as pd

# Function to read database (replace with actual database connection)
def read_database():
    # Your database connection logic goes here
    # This function should return a pandas dataframe

    # For demonstration, let's create a dataframe with some sample data
    data = {'ID': [1, 2, 3], 'Name': ['John', 'Alice', 'Bob']}
    return pd.DataFrame(data)

# Function to compare with expected output in Excel
def compare_with_excel(database_df, excel_file):
    # Read the Excel file into a pandas dataframe
    excel_df = pd.read_excel(excel_file)

    # Compare the data
    differences = database_df.compare(excel_df)

    return differences

# Function to print differences to an HTML file
def print_differences_html(differences, output_file):
    with open(output_file, 'w') as file:
        file.write("<html><head><style>table {border-collapse: collapse;} th, td {border: 1px solid black; padding: 8px;}</style></head><body>")
        file.write("<h2>Differences:</h2>")
        file.write(differences.to_html())
        file.write("</body></html>")

# Main function
def main():
    # Replace 'your_database_connection' with the actual function to read from your database
    database_df = read_database()

    # Replace 'your_expected_output.xlsx' with the path to your Excel file
    excel_file = 'your_expected_output.xlsx'

    # Replace 'differences.html' with the desired output file name
    output_file = 'differences.html'

    differences = compare_with_excel(database_df, excel_file)

    print_differences_html(differences, output_file)

if __name__ == "__main__":
    main()
