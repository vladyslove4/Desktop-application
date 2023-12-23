using DesktopApp.Command;
using DesktopApp.Domain.Interfaces;
using DesktopApp.Domain.Services;
using DesktopApp.Model.EntityDto;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopApp.Service
{
    public class DataService : ObservableObject
    {
        private ICourseService _courseService;
        private IGroupService _groupService;
        private IStudentService _studentService;
        private ITeacherService _teacherService;

        private ObservableCollection<CourseDto> _courses = new ObservableCollection<CourseDto>();
        private ObservableCollection<GroupDto> _groups = new ObservableCollection<GroupDto>();
        private ObservableCollection<StudentDto> _students = new ObservableCollection<StudentDto>();
        private ObservableCollection<TeacherDto> _teachers = new ObservableCollection<TeacherDto>();

        public ObservableCollection<GroupDto> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }

        public ObservableCollection<StudentDto> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public ObservableCollection<TeacherDto> Teachers
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
                OnPropertyChanged(nameof(Teachers));
            }
        }

        public ObservableCollection<CourseDto> Courses
        {
            get { return _courses; }

            set
            {
                _courses = value;
                OnPropertyChanged(nameof(Courses));
            }

        }

        public DataService(ICourseService courseService, IGroupService groupService,
                           IStudentService studentService, ITeacherService teacherService)
        {
            _courseService = courseService;
            _groupService = groupService;
            _studentService = studentService;
            _teacherService = teacherService;
        }

        public async Task LoadCoursesAsync(CancellationToken cancellationToken)
        {
            var courses = await _courseService.GetCoursesAsync(cancellationToken);
            _courses.Clear();
            foreach (var item in courses)
            {
                _courses.Add(DtoMapper.CourseToCourseDto(item));
            }
        }

        public async Task LoadGroupAsync(CancellationToken cancellationToken)
        {
            var groups = await _groupService.GetGroupsAsync(cancellationToken);
            _groups.Clear();
            foreach (var item in groups)
            {
                _groups.Add(DtoMapper.GroupToGroupDto(item));
            }
        }

        public async Task LoadStudentAsync(CancellationToken cancellationToken)
        {
            var students = await _studentService.GetStudentsAsync(cancellationToken);
            _students.Clear();
            foreach (var item in students)
            {
                _students.Add(DtoMapper.StudentToStudentDto(item));
            }
        }

        public async Task LoadTeacherAsync(CancellationToken cancellationToken)
        {
            var teachers = await _teacherService.GetTeachersAsync(cancellationToken);
            _teachers.Clear();
            foreach (var item in teachers)
            {
                _teachers.Add(DtoMapper.TeacherToTeacherDto(item));
            }
        }

        public async Task CreateCourseAsync(CourseDto course, CancellationToken cancellationToken)
        {
            var createdCourse = await _courseService.CreateCourseAsync(DtoMapper.CourseDtoToCourse(course), cancellationToken);

            _courses.Add(DtoMapper.CourseToCourseDto(createdCourse));

        }

        public async Task UpdateCourseAsync(CourseDto updCourse, CancellationToken cancellationToken)
        {
            await _courseService.UpdateCourseAsync(DtoMapper.CourseDtoToCourse(updCourse), cancellationToken);


            int index = Courses.IndexOf(Courses.FirstOrDefault(g => g.CourseId == updCourse.CourseId));

            if (index >= 0)
            {
                Courses[index] = updCourse;
            }

        }

        public async Task<bool> DeleteCourseAsync(CourseDto course, CancellationToken cancellationToken)
        {
            if (!await _courseService.DeleteCourseAsync(course.CourseId, cancellationToken))
            {

                return false;

            };

            Courses.Remove(course);
            return true;
        }

        public async Task CreateGroupAsync(GroupDto group, CancellationToken cancellationToken)
        {
            var createdGroup = await _groupService.CreateGroupAsync(DtoMapper.GroupDtoToGroup(group), cancellationToken);

            var teacher = await _teacherService.GetTeacherByIdAsync(createdGroup.TeacherId, cancellationToken);
            createdGroup.TeacherName = $"{teacher.Name} {teacher.LastName}";
            _groups.Add(DtoMapper.GroupToGroupDto(createdGroup));
        }

        public async Task UpdateGroupAsync(GroupDto updGroup, CancellationToken cancellationToken)
        {
            await _groupService.UpdateGroupAsync(DtoMapper.GroupDtoToGroup(updGroup), cancellationToken);

            int index = Groups.IndexOf(Groups.FirstOrDefault(g => g.GroupId == updGroup.GroupId));

            if (index >= 0)
            {
                Groups[index] = updGroup;
            }

        }

        public async Task<bool> DeleteGroupAsync(GroupDto group, CancellationToken cancellationToken)
        {
            if (!await _groupService.DeleteGroupAsync(group.GroupId, cancellationToken))
            {

                return false;

            };

            Groups.Remove(group);
            return true;
        }

        public async Task<GroupDto> GetDropdownForGroup(CancellationToken cancellationToken)
        {
            GroupDto? groupDto = new GroupDto();

            var courses = await _courseService.GetCoursesAsync(cancellationToken);
            var teachers = await _teacherService.GetTeachersAsync(cancellationToken);

            foreach (var course in courses)
            {
                DropDownItem item = new DropDownItem { Name = course.Name, Id = course.Id };
                groupDto.Courses.Add(item);
            };

            if (groupDto.Courses.Any())
            {
                groupDto.SelectedCourse = groupDto.Courses.FirstOrDefault();
            }

            foreach (var teacher in teachers)
            {
                DropDownItem item = new DropDownItem { Name = $"{teacher.Name + " " + teacher.LastName}", Id = teacher.Id };
                groupDto.Teachers.Add(item);
            };

            if (groupDto.Teachers.Any())
            {
                groupDto.SelectedTeacher = groupDto.Teachers.FirstOrDefault();
            }

            return groupDto;
        }

        public async Task CreateStudentAsync(StudentDto student, CancellationToken cancellationToken)
        {
            var createdStudent = await _studentService.CreateStudentAsync(DtoMapper.StudentDtoToStudent(student), cancellationToken);

            _students.Add(DtoMapper.StudentToStudentDto(createdStudent));
        }

        public async Task UpdateStudentAsync(StudentDto student, CancellationToken cancellationToken)
        {
            await _studentService.UpdateStudentAsync(DtoMapper.StudentDtoToStudent(student), cancellationToken);

            int index = Students.IndexOf(Students.FirstOrDefault(g => g.StudentId == student.StudentId));

            if (index >= 0)
            {
                Students[index] = student;
            }

        }

        public async Task<bool> DeleteStudentAsync(StudentDto student, CancellationToken cancellationToken)
        {
            await _studentService.DeleteStudentAsync(student.StudentId, cancellationToken);

            Students.Remove(student);

            return true;

        }

        public async Task<StudentDto> GetGroupsforStudent(CancellationToken cancellationToken)
        {
            StudentDto? studentpDto = new StudentDto();

            var groups = await _groupService.GetGroupsAsync(cancellationToken);

            foreach (var group in groups)
            {
                DropDownItem item = new DropDownItem { Name = group.Name, Id = group.Id };
                studentpDto.Groups.Add(item);
            };

            if (studentpDto.Groups.Any())
            {
                studentpDto.SelectedGroup = studentpDto.Groups.FirstOrDefault();
            }

            return studentpDto;
        }

        public async Task CreateTeacherAsync(TeacherDto teacher, CancellationToken cancellationToken)
        {
            var createdTeacher = await _teacherService.CreateTeacherAsync(DtoMapper.TeacherDtoToTeacher(teacher), cancellationToken);

            _teachers.Add(DtoMapper.TeacherToTeacherDto(createdTeacher));
        }

        public async Task UpdateTeacherAsync(TeacherDto teacher, CancellationToken cancellationToken)
        {
            await _teacherService.UpdateTeacherAsync(DtoMapper.TeacherDtoToTeacher(teacher), cancellationToken);

            int index = Teachers.IndexOf(Teachers.FirstOrDefault(g => g.TeacherId == teacher.TeacherId));

            if (index >= 0)
            {
                Teachers[index] = teacher;
            }

        }

        public async Task<bool> DeleteTeacherAsync(TeacherDto teacher, CancellationToken cancellationToken)
        {
            await _teacherService.DeleteTeacherAsync(teacher.TeacherId, cancellationToken);

            Teachers.Remove(teacher);

            return true;

        }

        public List<StudentDto> GetStudentsByGroupIdAsync(int groupId, CancellationToken cancellationToken)
        {

            var studentsById = Students.Where(s => s.GroupNumber == groupId)
                                      .OrderBy(s => s.GroupNumber)
                                      .ToList();

            return studentsById;
        }
    }
}