using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HudsonLimaAteliware.Models;
using System.Threading.Tasks;
using Octokit;
using System.Threading;
using Octokit.Internal;

namespace HudsonLimaAteliware.Controllers
{
    public class RepositsController : Controller
    {
        private RepositoryDBEntities db = new RepositoryDBEntities();
        Reposit repoCsharp;
        Reposit repoJavaScript;
        Reposit repoArduino;
        Reposit repoGroovy;
        Reposit repoRuby;

        // GET: Reposits
        public ActionResult Index()
        {
            return View(db.Reposits.ToList());
        }

        // GET: Reposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reposit reposit = db.Reposits.Find(id);
            if (reposit == null)
            {
                return HttpNotFound();
            }
            return View(reposit);
        }

        // GET: Reposits/Create
        public ActionResult Create()
        {
            return View();
        }
        


        /* Função que deleta os repositorios inseridos no banco, busca os repositórios de 5 linguagens com mais estrelas
           Utilizando Api Octokit para comunicação com Api do Github
        */
        public ActionResult Atualiza()
        {
            DateTime dataAgora = DateTime.Now;
            repoCsharp = new Reposit();
            repoJavaScript = new Reposit();
            repoArduino = new Reposit();
            repoGroovy  = new Reposit();
            repoRuby = new Reposit();


            var ghCliente = new GitHubClient(new ProductHeaderValue("HudsonLimaTesting"));
            ghCliente.Credentials = new Credentials(AppSettings.Settings.userName, AppSettings.Settings.password);

            Task task1 = new Task(() => ListUserReposAsync(ghCliente));
            task1.Start();

            //Deleta registros da tabela reposit
            DeleteReposits();

            while (repoCsharp.Name is null || repoJavaScript.Name is null || repoArduino.Name is null || repoGroovy.Name is null || repoRuby.Name is null) //
            {
                Thread.Sleep(1000);
            }

            InsereObj(repoCsharp, dataAgora);
            InsereObj(repoJavaScript, dataAgora);
            InsereObj(repoArduino, dataAgora);
            InsereObj(repoGroovy, dataAgora);
            InsereObj(repoRuby, dataAgora);

            TempData["Success"] = "Dados Atualizados com sucesso em "+ dataAgora.ToString();
            return RedirectToAction("Index");

        }

        private void DeleteReposits()
        {
            List<Reposit> reposits = db.Reposits.ToList();
            foreach (Reposit r in reposits)
            {
                db.Reposits.Remove(r);
                db.SaveChanges();
            }

        }

        private void InsereObj(Reposit r, DateTime dataAgora)
        {
            Reposit reposit = new Reposit();
            reposit.CreatedAt = r.CreatedAt;
            reposit.Description = r.Description;
            reposit.hasDownloads = r.hasDownloads;
            reposit.hasIssues = r.hasIssues;
            reposit.htmlURL = r.htmlURL;
            reposit.Language = r.Language;            
            reposit.UpdatedAt = r.UpdatedAt;
            reposit.InseridoNoBD = dataAgora;
            reposit.Stars = r.Stars;
            reposit.Name = r.Name;

            db.Reposits.Add(reposit);
            db.SaveChanges();
        }

        private async void ListUserReposAsync(GitHubClient ghCliente)
        {
         
            // //Populando Objeto de linguagem C#
            #region
            var searchRepositoriesRequest = new SearchRepositoriesRequest()
            {
                Language = Language.CSharp, 
                SortField = RepoSearchSort.Stars,                
                Order = SortDirection.Descending,
                PerPage = 1               
            };

            SearchRepositoryResult searchRepositoryResult = await ghCliente.Search.SearchRepo(searchRepositoriesRequest);
            
            repoCsharp.CreatedAt = searchRepositoryResult.Items[0].CreatedAt.DateTime;
            repoCsharp.Description = searchRepositoryResult.Items[0].Description;
            repoCsharp.hasDownloads = Convert.ToByte(searchRepositoryResult.Items[0].HasDownloads); 
            repoCsharp.hasIssues = Convert.ToByte(searchRepositoryResult.Items[0].HasIssues);
            repoCsharp.htmlURL = searchRepositoryResult.Items[0].HtmlUrl;
            repoCsharp.Language = searchRepositoryResult.Items[0].Language;
            repoCsharp.UpdatedAt = searchRepositoryResult.Items[0].UpdatedAt.DateTime;
            repoCsharp.Stars = searchRepositoryResult.Items[0].StargazersCount;
            repoCsharp.Name = searchRepositoryResult.Items[0].Name;
            
            #endregion
            
            //Populando Objeto de linguagem javaScript
            #region

            searchRepositoriesRequest = new SearchRepositoriesRequest()
            {
                Language = Language.JavaScript,
                SortField = RepoSearchSort.Stars,
                Order = SortDirection.Descending,
                PerPage = 1
            };

            searchRepositoryResult = await ghCliente.Search.SearchRepo(searchRepositoriesRequest);

            repoJavaScript.CreatedAt = searchRepositoryResult.Items[0].CreatedAt.DateTime;
            repoJavaScript.Description = searchRepositoryResult.Items[0].Description;
            repoJavaScript.hasDownloads = Convert.ToByte(searchRepositoryResult.Items[0].HasDownloads);
            repoJavaScript.hasIssues = Convert.ToByte(searchRepositoryResult.Items[0].HasIssues);
            repoJavaScript.htmlURL = searchRepositoryResult.Items[0].HtmlUrl;
            repoJavaScript.Language = searchRepositoryResult.Items[0].Language;
            repoJavaScript.UpdatedAt = searchRepositoryResult.Items[0].UpdatedAt.DateTime;
            repoJavaScript.Stars = searchRepositoryResult.Items[0].StargazersCount;
            repoJavaScript.Name = searchRepositoryResult.Items[0].Name;
            #endregion
            
            //Populando Objeto de linguagem Arduino(Wiring)
            #region
            searchRepositoriesRequest = new SearchRepositoriesRequest()
            {
                Language = Language.Arduino,
                SortField = RepoSearchSort.Stars,
                Order = SortDirection.Descending,
                PerPage = 1
            };

            searchRepositoryResult = await ghCliente.Search.SearchRepo(searchRepositoriesRequest);

            repoArduino.CreatedAt = searchRepositoryResult.Items[0].CreatedAt.DateTime;
            repoArduino.Description = searchRepositoryResult.Items[0].Description;
            repoArduino.hasDownloads = Convert.ToByte(searchRepositoryResult.Items[0].HasDownloads);
            repoArduino.hasIssues = Convert.ToByte(searchRepositoryResult.Items[0].HasIssues);
            repoArduino.htmlURL = searchRepositoryResult.Items[0].HtmlUrl;
            repoArduino.Language = searchRepositoryResult.Items[0].Language;
            repoArduino.UpdatedAt = searchRepositoryResult.Items[0].UpdatedAt.DateTime;
            repoArduino.Stars = searchRepositoryResult.Items[0].StargazersCount;
            repoArduino.Name = searchRepositoryResult.Items[0].Name;
            #endregion

            //Populando Objeto de linguagem Groovy
            #region
            searchRepositoriesRequest = new SearchRepositoriesRequest()
            {
                Language = Language.Groovy,
                SortField = RepoSearchSort.Stars,
                Order = SortDirection.Descending,
                PerPage = 1
            };

            searchRepositoryResult = await ghCliente.Search.SearchRepo(searchRepositoriesRequest);

            repoGroovy.CreatedAt = searchRepositoryResult.Items[0].CreatedAt.DateTime;
            repoGroovy.Description = searchRepositoryResult.Items[0].Description;
            repoGroovy.hasDownloads = Convert.ToByte(searchRepositoryResult.Items[0].HasDownloads);
            repoGroovy.hasIssues = Convert.ToByte(searchRepositoryResult.Items[0].HasIssues);
            repoGroovy.htmlURL = searchRepositoryResult.Items[0].HtmlUrl;
            repoGroovy.Language = searchRepositoryResult.Items[0].Language;
            repoGroovy.UpdatedAt = searchRepositoryResult.Items[0].UpdatedAt.DateTime;
            repoGroovy.Stars = searchRepositoryResult.Items[0].StargazersCount;
            repoGroovy.Name = searchRepositoryResult.Items[0].Name;
            #endregion

            //Populando Objeto de linguagem Ruby on Rails
            #region
            searchRepositoriesRequest = new SearchRepositoriesRequest()
            {
                Language = Language.Ruby,
                SortField = RepoSearchSort.Stars,
                Order = SortDirection.Descending,
                PerPage = 1
            };

            searchRepositoryResult = await ghCliente.Search.SearchRepo(searchRepositoriesRequest);

            repoRuby.CreatedAt = searchRepositoryResult.Items[0].CreatedAt.DateTime;
            repoRuby.Description = searchRepositoryResult.Items[0].Description;
            repoRuby.hasDownloads = Convert.ToByte(searchRepositoryResult.Items[0].HasDownloads);
            repoRuby.hasIssues = Convert.ToByte(searchRepositoryResult.Items[0].HasIssues);
            repoRuby.htmlURL = searchRepositoryResult.Items[0].HtmlUrl;
            repoRuby.Language = searchRepositoryResult.Items[0].Language;
            repoRuby.UpdatedAt = searchRepositoryResult.Items[0].UpdatedAt.DateTime;
            repoRuby.Stars = searchRepositoryResult.Items[0].StargazersCount;
            repoRuby.Name = searchRepositoryResult.Items[0].Name;
            
            #endregion
                       

        }




        // POST: Reposits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,hasDownloads,hasIsues,htmlURL,Language,CreatedAt,UpdatedAt")] Reposit reposit)
        {
            if (ModelState.IsValid)
            {
                db.Reposits.Add(reposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reposit);
        }

        // GET: Reposits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reposit reposit = db.Reposits.Find(id);
            if (reposit == null)
            {
                return HttpNotFound();
            }
            return View(reposit);
        }

        // POST: Reposits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,hasDownloads,hasIsues,htmlURL,Language,CreatedAt,UpdatedAt")] Reposit reposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reposit);
        }

        // GET: Reposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reposit reposit = db.Reposits.Find(id);
            if (reposit == null)
            {
                return HttpNotFound();
            }
            return View(reposit);
        }

        // POST: Reposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reposit reposit = db.Reposits.Find(id);
            db.Reposits.Remove(reposit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
