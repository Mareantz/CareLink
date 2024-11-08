namespace Domain.Enums
{
    public enum UserRole
    {
        None = 0,
        Doctor = 1, // DOAR ADMINUL POATE SA SETEZE
        Patient = 2,
        Admin = 3
    }
}
