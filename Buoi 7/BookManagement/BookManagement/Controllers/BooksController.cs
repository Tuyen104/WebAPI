using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BookManagement.Models;

namespace BookManagement.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        private BookManagementEntities db = new BookManagementEntities();

        // GET: api/Books
        public IQueryable<object> GetBooks()
        {
            var book = from p in db.Books
                       select new
                       {
                           p.Id,
                           p.Title,
                           p.Price,
                           p.Author.FullName
                       };

            return book;
        }

        // GET: api/Books/5
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            var book = from p in db.Books
                       where p.Id == id
                       select new
                       {
                           p.Id,
                           p.Title,    
                           p.Price,
                           p.Author.FullName
                       };
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // GET: api/Books/5/Details
        [Route("{id:int}/details")]
        [ResponseType(typeof(object))]
        public async Task<IHttpActionResult> GetBookDetail(int id)
        {
            var book = from p in db.Books
                       where p.Id == id
                       select new
                       {
                           p.Id,
                           p.Title,
                           p.Price,
                           p.Author.FullName,
                           p.Genre,
                           p.Description,
                           p.PublicationDate
                       };
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        // GET: api/Books/genre
        [Route("{genre}")]
        public IQueryable<object> GetBooksByGenre(string genre)
        {
            var book = from p in db.Books
                       where p.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)
                       select new
                       {
                           p.Id,
                           p.Title,
                           p.Price,
                           p.Author.FullName
                       };

            return book;
        }
        //GET: /api/authors/1/books
        [Route("~/api/authors/{authorID:int}/books")]
        public IQueryable<object> GetBooksByAuthor(int authorID)
        {
            var book = from b in db.Books
                       where b.AuthorID == authorID
                       select new
                       {
                           b.Id,
                           b.Title,
                           b.Price,
                           b.PublicationDate
                       };
            return book;
        }
         //GET: /api/books/date/yyyy-mm-dd
         [Route("date/{publishDate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
         public IQueryable<object> GetBooksByPubDate(DateTime publishDate)
        {
            var book = from b in db.Books
                       where b.PublicationDate == publishDate
                       select new
                       {
                           b.Id,
                           b.Title,
                           b.Price,
                           b.PublicationDate
                       };
            return book;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.Id == id) > 0;
        }
    }
}