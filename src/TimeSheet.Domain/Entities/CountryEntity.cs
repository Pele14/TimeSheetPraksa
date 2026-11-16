namespace Timesheet.Domain.Entities;

    public class CountryEntity
    {
        public Guid Id { get; set; }
        public  string Name { get; set; }

        public  string Code { get; set; }


        public CountryEntity(Guid id, string name, string code)
    {
        Id=id;
        Name=name;
        Code=code;

    }
    }
