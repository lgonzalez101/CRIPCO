using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRIPCO.BD;
using CRIPCO.Models;
using System.Text;

namespace CRIPCO.Controllers
{
    public class CmsArticulosController : Controller
    {

        #region Funciones
        private IEnumerable<SelectListItem> GetsTipos()
        {
            var tipos = new List<SelectListItem>(){
                         new SelectListItem() {Text="Menu", Value="M"},
                         new SelectListItem() {Text="Articulo", Value="A"}
                            };
            return tipos;
        }

        private IEnumerable<SelectListItem> GetListParent()
        {
            var cmsArticulo = db.CmsArticulos.ToList();

            var Parents = cmsArticulo.Select(x =>
                                  new SelectListItem
                                  {
                                      Value = x.ArticuloId.ToString(),
                                      Text = x.Titulo,
                                  });
            return Parents;
        }

        private List<TreeNodeModels> FillRecursive(List<CmsArticulosModels> flatObjects, int? parentId = null)
        {
            return flatObjects.Where(x => x.ParentArticuloId.Equals(parentId)).Select(item => new TreeNodeModels
            {
                ArticuloName = item.ArticuloName,
                ArticuloId = item.ArticuloId,
                Tipo = item.SelectedTipo,
                Children = FillRecursive(flatObjects, item.ArticuloId)
            }).ToList();
        }

        public void RecursiveLectura(ref string root_li, ref int level, List<TreeNodeModels> headerTree)
        {

            foreach (var item in headerTree)
            {
                root_li += "<li role=\"treeitem\" aria-selected=\"false\" aria -level=\"" + level + "\" aria -labelledby=\"j1_11_anchor\" id =\"j1_11\" class=\"jstree - node  jstree - leaf jstree - last\" >";

                if (item.Tipo.Trim() == "M")
                {
                    root_li += "<a class=\"jstree-anchor\" href=\"/CmsArticulos/Edit/" + item.ArticuloId + "\" tabindex=\"-1\" id=\"j1_11_anchor\">" +
                               "<i class=\"jstree-icon jstree-themeicon fa fa-folder text-warning fa-lg jstree-themeicon-custom\" role =\"presentation\" ></i>" + item.ArticuloName + "</a>";
                }
                else
                {
                    root_li += " <a class=\"jstree -anchor\" href=\"/CmsArticulos/Edit/" + item.ArticuloId + "\" tabindex=\"-1\" id=\"j1_10_anchor\" >" +
                         "<i class=\"jstree -icon jstree-themeicon fa fa-link fa-lg text-primary jstree-themeicon-custom\" role =\"presentation\" ></i>" +
                             item.ArticuloName + "</a>";
                }
                if (item.Children.Count > 0)
                {
                    root_li += "<ul role=\"group\" class=\"jstree - children\" style=\"\">";
                    level += 1;
                    RecursiveLectura(ref root_li, ref level, item.Children);
                    root_li += "</ul>";
                }
                root_li += "</ li >";
            }
        }

        public void RecursiveLectura(ref string root_li , List<TreeNodeModels> headerTree)
        {

            foreach (var item in headerTree)
            { 
                if (item.Tipo.Trim() == "M")
                {

                    root_li += "<li class=\"dropdown\">< a href=\"#home\" data-click=\"scroll-to-target\" data-target=\"#\" data-toggle=\"dropdown\" >" + item.ArticuloName + "<b class=\"caret\"></b></a>";
                        }
                else
                {
                    root_li += "<li><a href=\"index.html\"> "+item.ArticuloName+" </a ></li>";
                }
                if (item.Children.Count > 0)
                {
                    root_li += "  <ul class=\"dropdown-menu dropdown-menu-left animated fadeInDown\">";

                    RecursiveLectura(ref root_li, item.Children);
                    root_li += "</ul>";
                }
                root_li += "</li>";
            }
        }

        [HttpGet]
        public ActionResult DetalleArticulo()
        {
            DetalleArticuloModels model = new DetalleArticuloModels();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult DetalleArticulo(DetalleArticuloModels category, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //image.ContentType;
                    int length = image.ContentLength;
                    byte[] buffer = new byte[length];
                    image.InputStream.Read(buffer, 0, length);
                   //ategory.Imagen = buffer;
                }

                //db.Categories.AddObject(category);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return PartialView(category);
        }

        public ActionResult empty()
        {
            return PartialView();
        }

        public string GetAllCategoriesForTree()
        {
            List<CmsArticulosModels> articulos = new List<CmsArticulosModels>();
            var dt = db.CmsArticulos.ToList();

            if (dt != null && dt.Count > 0)
            {
                foreach (var row in dt)
                {
                    articulos.Add(
                        new CmsArticulosModels
                        {
                            ArticuloId = row.ArticuloId,
                            ArticuloName = row.Titulo,
                            SelectedTipo = row.Tipo,
                            ParentArticuloId = (row.PadreArticuloId != 0) ? row.PadreArticuloId : (int?)null
                        });
                }

                List<TreeNodeModels> headerTree = FillRecursive(articulos, null);

                string root_li = string.Empty;
                int level = 1;
                RecursiveLectura(ref root_li, ref level, headerTree);
                return root_li;
            }
            return "Record Not Found!!";
        }

        #endregion
        private CripcoEntities db = new CripcoEntities();

        public string Descripcion { get; private set; }

        public ActionResult VerContenido()
        {
            List<CmsArticulosModels> articulos = new List<CmsArticulosModels>();
   
                foreach (var row in db.CmsArticulos.ToList())
                {
                    articulos.Add(
                        new CmsArticulosModels
                        {
                            ArticuloId = row.ArticuloId,
                            ArticuloName = row.Titulo,
                            SelectedTipo = row.Tipo,
                            ParentArticuloId = (row.PadreArticuloId != 0) ? row.PadreArticuloId : (int?)null
                        });
                }
                List<TreeNodeModels> headerTree = FillRecursive(articulos, null);
           // string root_li = string.Empty;
        //    RecursiveLectura(ref root_li, headerTree);
         //   ViewBag.Menu = root_li;
            return View(headerTree);
        }

        public ActionResult MostrarContenido(int Id)
        {
            var articulo = db.CmsArticulos.Find(Id);
            return PartialView(articulo);
        }

        // GET: CmsArticulos
        public ActionResult Index()
        {
            ViewBag.Tree = GetAllCategoriesForTree();
            return View();
        }

        // GET: CmsArticulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cmsArticulo = db.CmsArticulos.Find(id);

            if (cmsArticulo == null)
            {
                return HttpNotFound();
            }

            var cmsArticulosModels = new CmsArticulosModels
            {
                ArticuloId = cmsArticulo.ArticuloId,
                ArticuloName = cmsArticulo.Titulo,
                ParentArticuloId = cmsArticulo.PadreArticuloId,
                ListParent = GetListParent(),
                SelectedTipo = cmsArticulo.Tipo,
                Tipos = GetsTipos()
            };

            return View(cmsArticulosModels);
        }

        // GET: CmsArticulos/Create
        public ActionResult Create()
        {
            var cmsArticulosModels = new CmsArticulosModels
            {
                ParentArticuloId = 0,
                ListParent = GetListParent(),
                SelectedTipo = "A",
                Tipos = GetsTipos()
            };

            return View(cmsArticulosModels);
        }

        // POST: CmsArticulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CmsArticulosModels cmsArticulosModels)
        {
            cmsArticulosModels.ListParent = GetListParent();
            cmsArticulosModels.Tipos = GetsTipos();

            if (ModelState.IsValid)
            {
                if (cmsArticulosModels.SelectedTipo != "M")
                {
                    if (string.IsNullOrEmpty(cmsArticulosModels.Descripcion))
                    {
                        ViewBag.ErroMgs = "El texto para mostrar no puede estar vacio...";
                        return View(cmsArticulosModels);
                    }
                }

                var cmsArticulosDetalle = new List<CmsArticulosDetalle>();
                foreach (var file in cmsArticulosModels.Files)
                {
                    int length = file.ContentLength;
                    byte[] buffer = new byte[length];
                    file.InputStream.Read(buffer, 0, length);
                    cmsArticulosDetalle.Add(new CmsArticulosDetalle { Imagen = buffer, UrlVideo = file.FileName, ArticuloId = cmsArticulosModels.ArticuloId });
                }

                var _cmsArticulos = new CRIPCO.BD.CmsArticulos
                {
                    ArticuloId = cmsArticulosModels.ArticuloId,
                    Descripcion = cmsArticulosModels.Descripcion,
                    PadreArticuloId = (int)cmsArticulosModels.ParentArticuloId,
                    Tipo = cmsArticulosModels.SelectedTipo,
                    Titulo = cmsArticulosModels.ArticuloName,
                    CmsArticulosDetalle = cmsArticulosDetalle

                };
                db.CmsArticulos.Add(_cmsArticulos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cmsArticulosModels);

        }

        // GET: CmsArticulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cmsArticulo = db.CmsArticulos.Include(x=>x.CmsArticulosDetalle).SingleOrDefault(x => x.ArticuloId == id);

            if (cmsArticulo == null)
            {
                return HttpNotFound();
            }

            var cmsArticulosModels = new CmsArticulosModels
            {
                ArticuloId = cmsArticulo.ArticuloId,
                ArticuloName = cmsArticulo.Titulo,
                ParentArticuloId = cmsArticulo.PadreArticuloId,
                Descripcion = cmsArticulo.Descripcion,
                ListParent = GetListParent(),
                SelectedTipo = cmsArticulo.Tipo.Trim(),
                Tipos = GetsTipos()
            };


            foreach (var r in cmsArticulo.CmsArticulosDetalle)
            {
                string base64String = Convert.ToBase64String(r.Imagen);

                cmsArticulosModels.detalleArticuloModels.Add(new DetalleArticuloModels { ArticuloDetalleId= r.ArticuloDetalleId,
                    ArticuloId =r.ArticuloId,Imagen= base64String, UrlVideo= r.UrlVideo });
            }


            return View(cmsArticulosModels);
        }
        // POST: CmsArticulos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CmsArticulosModels cmsArticulosModels)
        {
            cmsArticulosModels.ListParent = GetListParent();
            cmsArticulosModels.Tipos = GetsTipos();

            if (ModelState.IsValid)
            {
                if (cmsArticulosModels.SelectedTipo != "M")
                {
                    if (string.IsNullOrEmpty(cmsArticulosModels.Descripcion))
                    {
                        ViewBag.ErroMgs = "El texto para mostrar no puede estar vacio...";
                        return View(cmsArticulosModels);
                    }
                }
                var _cmsArticulos = db.CmsArticulos.Find(cmsArticulosModels.ArticuloId);


                var cmsArticulosDetalle = new List<CmsArticulosDetalle>();

                foreach (var file in cmsArticulosModels.Files)
                {
                    int length = file.ContentLength;
                    byte[] buffer = new byte[length];
                    file.InputStream.Read(buffer, 0, length);
                    cmsArticulosDetalle.Add(new CmsArticulosDetalle { Imagen = buffer, UrlVideo = file.FileName, ArticuloId = _cmsArticulos.ArticuloId });
                }

                foreach (var artDet in cmsArticulosModels.detalleArticuloModels)
                {
                    byte[] bArray = System.Convert.FromBase64String(artDet.Imagen);
                    cmsArticulosDetalle.Add(new CmsArticulosDetalle { Imagen = bArray, UrlVideo = artDet.UrlVideo, ArticuloId = _cmsArticulos.ArticuloId });
                }


                var query = (from p in db.CmsArticulosDetalle
                             where p.ArticuloId == _cmsArticulos.ArticuloId
                             select p);

                db.CmsArticulosDetalle.RemoveRange(query);
                db.SaveChanges();


                _cmsArticulos.ArticuloId = cmsArticulosModels.ArticuloId;
                _cmsArticulos.Descripcion = cmsArticulosModels.Descripcion;
                _cmsArticulos.PadreArticuloId = (int)cmsArticulosModels.ParentArticuloId;
                _cmsArticulos.Tipo = cmsArticulosModels.SelectedTipo;
                _cmsArticulos.Titulo = cmsArticulosModels.ArticuloName;
                _cmsArticulos.CmsArticulosDetalle = cmsArticulosDetalle;

                db.Entry(_cmsArticulos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(cmsArticulosModels);
        }

        // GET: CmsArticulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CmsArticulos cmsArticulos = db.CmsArticulos.Find(id);
            if (cmsArticulos == null)
            {
                return HttpNotFound();
            }
            return View(cmsArticulos);
        }

        // POST: CmsArticulos/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            CmsArticulos cmsArticulos = db.CmsArticulos.Find(id);
            db.CmsArticulos.Remove(cmsArticulos);
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
