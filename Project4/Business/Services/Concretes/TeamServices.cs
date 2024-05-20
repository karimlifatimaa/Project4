using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class TeamServices : ITeamServices
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeamServices(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment )
        {
            _teamRepository = teamRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddTeam(Team team)
        {
            if (team == null) 
                throw new EntityNullException("Entity not found");
            if(team.PhotoFile==null )
                throw new EntityNullException("Entity not found");
            if (!team.PhotoFile.ContentType.Contains("image/")) 
                throw new FileContentTypeException("PhotoFile","Content type error");
            if (team.PhotoFile.Length > 3000000)
                throw new FileSizeErrorException("PhotoFIle", "File size error");
            string path = _webHostEnvironment.WebRootPath + @"\Uploads\Team\" + team.PhotoFile.FileName;
            using(FileStream stream=new FileStream(path, FileMode.Create))
            {
                team.PhotoFile.CopyTo(stream);
            }
            team.ImgUrl = team.PhotoFile.FileName;
            _teamRepository.Add(team);
            _teamRepository.Commit();
               
        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _teamRepository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _teamRepository.Get(func);
        }

        public void RemoveTeam(int id)
        {
            var team=_teamRepository.Get(x=> x.Id == id);   
            if(team == null)
                throw new EntityNullException("Entity not found");

            string path = _webHostEnvironment.WebRootPath + @"\Uploads\Team\" + team.ImgUrl;
            if(!File.Exists(path))
                throw new EntityNullException("Entity not found");
            File.Delete(path);
            _teamRepository.Delete(team);
            _teamRepository.Commit();

        }

        public void UpdateTeam(int id, Team team)
        {
            var oldTeam = _teamRepository.Get(x => x.Id == id);
            if (oldTeam == null)
                throw new EntityNullException("Entity not found");
            if (team.PhotoFile != null)
            {
                if (!team.PhotoFile.ContentType.Contains("image/"))
                    throw new FileContentTypeException("PhotoFile", "Content type error");
                if(team.PhotoFile.Length>3000000)
                    throw new FileSizeErrorException("PhotoFIle", "File size error");
                string path1 = _webHostEnvironment.WebRootPath + @"\Uploads\Team\" + oldTeam.ImgUrl;
                if (!File.Exists(path1))
                    throw new EntityNullException("Entity not found");
                File.Delete(path1);
                string path = _webHostEnvironment.WebRootPath + @"\Uploads\Team\" + team.PhotoFile.FileName;
                using(FileStream stream=new FileStream(path, FileMode.Create))
                {
                    team.PhotoFile.CopyTo(stream);
                }
                oldTeam.ImgUrl = team.PhotoFile.FileName;
            }
            oldTeam.FullName= team.FullName;
            oldTeam.Description = team.Description;
            oldTeam.Position = team.Position;
            _teamRepository.Commit();

        }
    }
}
