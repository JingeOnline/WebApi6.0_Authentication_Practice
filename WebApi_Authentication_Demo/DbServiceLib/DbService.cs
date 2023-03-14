using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib
{
    public class DbService : IDbService
    {
        private static StudentManagementDbContext _context = new StudentManagementDbContext();


        public List<Student> GetStudentsAll()
        {
            return _context.Students.ToList();
        }
        public List<Subject> GetSubjectsAll()
        {
            return _context.Subjects.ToList();
        }
        public Student GetStudent(int pkid)
        {
            return _context.Students.FirstOrDefault(x => x.PkId == pkid);

        }
        public Subject GetSubject(int pkid)
        {
            return _context.Subjects.FirstOrDefault(x => x.PkId == pkid);
        }
        public Student AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }
        public Subject AddSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
            return subject;
        }
        public Student UpdateStudent(Student student)
        {
            Student stu = _context.Students.FirstOrDefault(x => x.PkId == student.PkId);
            if (stu != null)
            {
                stu.Name = student.Name;
                stu.Subjects = student.Subjects;
                stu.Gender = student.Gender;
                stu.Age = student.Age;
                _context.SaveChanges();
                return stu;
            }
            else
            {
                return null;
            }
        }
        public bool RemoveStudent(int pkid)
        {
            Student student = _context.Students.FirstOrDefault(x => x.PkId == pkid);
            if (student != null)
            {
                try
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                    return true;
                }
                catch
                { 
                    return false; 
                }
            }
            else
            {
                return false;
            }
        }
        public bool RemoveSubject(int pkid)
        {
            Subject subject= _context.Subjects.FirstOrDefault(x => x.PkId == pkid);
            if(subject != null)
            {
                try
                {
                    _context.Subjects.Remove(subject);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
