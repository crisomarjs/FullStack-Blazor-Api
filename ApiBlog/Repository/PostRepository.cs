using ApiBlog.Data;
using ApiBlog.Models;
using ApiBlog.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ActualizarPost(Post post)
        {
            post.FechaActualizacion = DateTime.Now;
            var imagen = _context.Post.AsNoTracking().FirstOrDefault(p => p.Id == post.Id);

            if (post.RutaImagen == null)
            {
                post.RutaImagen = imagen.RutaImagen;
            }
            _context.Post.Update(post);
            return Guardar();
        }

        public bool BorrarPost(Post post)
        {
            _context.Post.Remove(post);
            return Guardar();
        }

        public bool CrearPost(Post post)
        {
            post.FechaCreacion = DateTime.Now;
            _context.Post.Add(post);
            return Guardar();
        }

        public bool ExistePost(string nombre)
        {
            bool valor = _context.Post.Any(p => p.Titulo.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExistePost(int id)
        {
            return _context.Post.Any(p => p.Id == id);
        }

        public Post GetPost(int id)
        {
            return _context.Post.FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Post> GetPosts()
        {
            return _context.Post.OrderBy(p => p.Id).ToList();
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}
