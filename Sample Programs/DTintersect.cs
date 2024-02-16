using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;

namespace DataTableComparisonTests
{
    [TestClass]
    public class DataTableComparisonTest
    {
        [TestMethod]
        public void TestStudentsEnrolledOnlyForMaths()
        {
            DataTable dtMaths = GetMathsDataTable();
            DataTable dtEnglish = GetEnglishDataTable();

            DataTable dtOnlyMaths = dtMaths.AsEnumerable().Except(dtEnglish.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();

            Assert.AreEqual(1, dtOnlyMaths.Rows.Count, "Mismatch in number of students enrolled only for Maths");

            foreach (DataRow dr in dtOnlyMaths.Rows)
            {
                Assert.Fail($"StudentID: {dr[0]}, StudentName: {dr[1]}");
            }
        }

        [TestMethod]
        public void TestStudentsEnrolledOnlyForEnglish()
        {
            DataTable dtMaths = GetMathsDataTable();
            DataTable dtEnglish = GetEnglishDataTable();

            DataTable dtOnlyEnglish = dtEnglish.AsEnumerable().Except(dtMaths.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();

            Assert.AreEqual(1, dtOnlyEnglish.Rows.Count, "Mismatch in number of students enrolled only for English");

            foreach (DataRow dr in dtOnlyEnglish.Rows)
            {
                Assert.Fail($"StudentID: {dr[0]}, StudentName: {dr[1]}");
            }
        }

        [TestMethod]
        public void TestStudentsEnrolledForBothMathAndEnglish()
        {
            DataTable dtMaths = GetMathsDataTable();
            DataTable dtEnglish = GetEnglishDataTable();

            DataTable dtIntersect = dtMaths.AsEnumerable().Intersect(dtEnglish.AsEnumerable(), DataRowComparer.Default).CopyToDataTable();

            Assert.AreEqual(2, dtIntersect.Rows.Count, "Mismatch in number of students enrolled for both Math and English");

            foreach (DataRow dr in dtIntersect.Rows)
            {
                Assert.Fail($"StudentID: {dr[0]}, StudentName: {dr[1]}");
            }
        }

        private DataTable GetMathsDataTable()
        {
            DataTable dtMaths = new DataTable("Maths");
            dtMaths.Columns.Add("StudID", typeof(int));
            dtMaths.Columns.Add("StudName", typeof(string));
            dtMaths.Rows.Add(1, "Mike");
            dtMaths.Rows.Add(2, "Mukesh");
            dtMaths.Rows.Add(3, "Erin");
            dtMaths.Rows.Add(4, "Roshni");
            dtMaths.Rows.Add(5, "Ajay");
            return dtMaths;
        }

        private DataTable GetEnglishDataTable()
        {
            DataTable dtEnglish = new DataTable("English");
            dtEnglish.Columns.Add("StudID", typeof(int));
            dtEnglish.Columns.Add("StudName", typeof(string));
            dtEnglish.Rows.Add(6, "Arjun");
            dtEnglish.Rows.Add(2, "Mukesh");
            dtEnglish.Rows.Add(7, "John");
            dtEnglish.Rows.Add(4, "Roshni");
            dtEnglish.Rows.Add(8, "Kumar");
            return dtEnglish;
        }
    }
}
