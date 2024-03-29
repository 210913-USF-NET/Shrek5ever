using System;
using System.Collections.Generic;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;

namespace DL
{
    public class DBRepo : IRepo
    {
        private SCDBContext _context;

        public DBRepo(SCDBContext context)
        {
            _context = context;
        }

        public void AddObject(Object thing)
        {
            /// Adds an object to the appropriate database. 
            /// thing is the object being added
            
            thing = _context.Add(thing).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void DeleteObject(Client thing)
        {
            /// Deletes a mofo

            _context.Clients.Remove(thing);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
        public void DeleteObject(Weight thing)
        {
            /// Deletes a mofo

            _context.Weights.Remove(thing);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
        public void DeleteObject(Exercise thing)
        {
            /// Deletes a mofo

            _context.Exercises.Remove(thing);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void Update(Client thing)
        {
            /// Updates an object in the appropriate database. 
            /// thing is the object being updated

            thing = _context.Clients.Update(thing).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void Update(Weight thing)
        {
            /// Updates an object in the appropriate database. 
            /// thing is the object being updated

            thing = _context.Weights.Update(thing).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public void Update(Exercise thing)
        {
            /// Updates an object in the appropriate database. 
            /// thing is the object being updated

            thing = _context.Exercises.Update(thing).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public Client GetOneClient(int Id)
        {
            /// Gets one client from all the clients
            /// Id is the ID of the client you want

            Client client = _context.Clients
                .Where(i => i.Id == Id)
                .Select(
                c => new Client()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Password = c.Password
                }
                ).SingleOrDefault();

            if (client is not null)
                return client;
            else
                return null;
        }

        public Client GetOneClient(string first, string last)
        {
            /// Gets one client from all the clients
            /// Id is the ID of the client you want

            Client client = _context.Clients
                .Where(i => i.FirstName == first)
                .Where(i => i.LastName == last)
                .Select(
                c => new Client()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Password = c.Password
                }
                ).SingleOrDefault();

            if (client is not null)
                return client;
            else
                return null;
        }

            public List<Client> GetAllClients()
        {
            /// Gets all the clients in a list
            
            return _context.Clients
                .Select(
                c => new Client()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Password = c.Password
                }
            ).ToList();
        }

        public List<Weight> GetAllWeights()
        {
            /// Gets all the weights in a list

            return _context.Weights
                .Select(
                w => new Weight()
                {
                    Id = w.Id,
                    DateTime = w.DateTime,
                    Amount = w.Amount,
                    ClientId = w.ClientId,
                    ExerciseId = w.ExerciseId
                }
            ).ToList();
        }

        public List<Exercise> GetAllExercises()
        {
            /// Gets all the exercises in a list

            return _context.Exercises
                .Select(
                e => new Exercise()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description
                }
            ).ToList();
        }

        public List<Weight> GetWeights(Client c)
        {
            /// Gets all the exercises in a list
            return (from w in _context.Weights
                    where c.Id == w.ClientId
                    select w).ToList();
        }

        public List<Weight> GetWeightsByClient(int Id)
        {
            /// Gets all the weights with respect to a client
            /// Id is the client you want

            return _context.Weights
                .Where(i => i.ClientId == Id)
                .Select(
                w => new Weight()
                {
                    Id = w.Id,
                    DateTime = w.DateTime,
                    Amount = w.Amount,
                    ClientId = w.ClientId,
                    ExerciseId = w.ExerciseId
                }
            ).ToList();
        }

        public Exercise GetExerciseById(int Id)
        {
            /// Gets all the weights with respect to a client
            /// Id is the client you want

            return _context.Exercises
                .Where(i => i.Id == Id)
                .Select(
                w => new Exercise()
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description
                }
            ).SingleOrDefault();
        }
        public List<Exercise> GetExerciseByWeightByClient(int Id)
        {
            /// Gets all the exercises with respect to a client
            /// Id is the client you want
            
            List<Weight> WeightsByClient = GetWeightsByClient(Id);

            List<Exercise> exercisesByWeightsByClients = new List<Exercise>();

            List<Exercise> allExercises = GetAllExercises();

            foreach (Weight wbC in WeightsByClient)
            {
                foreach (Exercise item in allExercises)
                {
                    if (item.Id == wbC.ExerciseId)
                        exercisesByWeightsByClients.Add(item);
                }
            }

            return exercisesByWeightsByClients;
        }
    }
}