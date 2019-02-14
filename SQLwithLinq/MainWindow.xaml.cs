using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Configuration;

namespace SQLwithLinq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["SQLwithLinq.Properties.Settings.PremSqlDBConnectionString"].ConnectionString;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            //InsertUniversities();
            //InsertStudents();
            //InsertLectures();
            //InsertStudentLectureAssociations();
            //GetUniversityOfTony();
            //GetLecturesForTony();
        }

        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("DELETE FROM University");

            University yale = new University();
            yale.Name = "Yale";
            dataContext.Universities.InsertOnSubmit(yale);

            University bejingTech = new University();
            bejingTech.Name = "Bejing Tech";
            dataContext.Universities.InsertOnSubmit(bejingTech);


            dataContext.SubmitChanges();

            mainDataGrid.ItemsSource = dataContext.Universities;
        }

        /// <summary>
        /// Insert Student to database
        /// </summary>
        public void InsertStudents()
        {
            University yale = dataContext.Universities.First(uni => uni.Name.Equals("Yale"));
            University bejing = dataContext.Universities.First(uni => uni.Name.Equals("Bejing Tech"));

            List<Student> students = new List<Student>
            {
                new Student { Name = "Carla", Gender = "Female", UniversityId = yale.Id },
                new Student { Name = "Tony", Gender = "Male", University = yale },
                new Student { Name = "Samantha", Gender = "Female", University = bejing },
                new Student { Name = "Sam", Gender = "Male", University = bejing },
                new Student { Name = "James", Gender = "Trans Gender", University = yale }
            };

            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();

            mainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Mathamatics" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });

            dataContext.SubmitChanges();

            mainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Tony = dataContext.Students.First(st => st.Name.Equals("Tony"));
            Student Samantha = dataContext.Students.First(st => st.Name.Equals("Samantha"));
            Student Sam = dataContext.Students.First(st => st.Name.Equals("Sam"));
            Student James = dataContext.Students.First(st => st.Name.Equals("James"));

            Lecture Mathamatics = dataContext.Lectures.First(lc => lc.Name.Equals("Mathamatics"));
            Lecture History = dataContext.Lectures.First(lc => lc.Name.Equals("History"));

            // this way
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Carla, Lecture = Mathamatics });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Tony, Lecture = History });
           
            // or this way
            StudentLecture slSamantha = new StudentLecture
            {
                Student = Samantha,
                LectureId = Mathamatics.Id
            };
            dataContext.StudentLectures.InsertOnSubmit(slSamantha);

            StudentLecture slSam = new StudentLecture
            {
                Student = Sam,
                LectureId = History.Id
            };
            dataContext.StudentLectures.InsertOnSubmit(slSam);

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = James, Lecture = Mathamatics });


            dataContext.SubmitChanges();

            mainDataGrid.ItemsSource = dataContext.StudentLectures;

        }

        public void GetUniversityOfTony()
        {
            Student Tony = dataContext.Students.First(st => st.Name.Equals("Tony"));
            University TonyUniversity = Tony.University;

            List<University> universities = new List<University>();
            universities.Add(TonyUniversity);

            mainDataGrid.ItemsSource = universities;
        }

        public void GetLecturesForTony()
        {

            Student Tony = dataContext.Students.First(st => st.Name.Equals("Tony"));

            var TonyLecture = from sl in Tony.StudentLectures select sl.Lecture;

            mainDataGrid.ItemsSource = TonyLecture;
        }
    }
}
