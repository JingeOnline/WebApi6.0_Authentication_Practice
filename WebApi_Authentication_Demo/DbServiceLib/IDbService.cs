using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib
{
    public interface IDbService
    {
        List<Student> GetStudentsAll();
        List<Subject> GetSubjectsAll();
        Student GetStudent(int pkid);
        Subject GetSubject(int pkid);
        Student AddStudent(Student student);
        Subject AddSubject(Subject subject);
        Student UpdateStudent(Student student);
        Subject UpdateSubject(Subject subject);
        string RemoveStudent(int pkid);
        string RemoveSubject(int pkid);
        void RemoveStudent(Student student);
        void RemoveSubject(Subject subject);
    }
}
