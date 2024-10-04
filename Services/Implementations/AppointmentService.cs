using BB.APIs.BeardBoss.Monolithic.Data;
using BB.APIs.BeardBoss.Monolithic.Models;
using BB.APIs.BeardBoss.Monolithic.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BB.APIs.BeardBoss.Monolithic.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            // Validação de data (evitar datas no passado)
            if (appointment.AppointmentDate < DateTime.Now)
            {
                throw new InvalidOperationException("The appointment date cannot be in the past.");
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }

}
