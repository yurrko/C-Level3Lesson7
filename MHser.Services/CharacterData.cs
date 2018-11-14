using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MHser.Services
{
    public class CharacterData : ICharactersData
    {
        public IEnumerable<Character> GetCharacters()
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Characters.ToList();
            }
        }
        public Character GetCharacterById( int id )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Characters.FirstOrDefault( c => c.CharacterId == id );
            }
        }
        public Character AddCharacter( Character character )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Characters.Add( character );
                context.SaveChanges();
                return character;
            }
        }
        public void ModifyCharacter( Character character )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Characters.Attach( character );
                context.Entry( character ).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCharacter( int id )
        {
            var characterToDelete = GetCharacterById( id );

            if ( characterToDelete is null ) return;

            using ( var context = new MessoyahaContext() )
            {
                context.Characters.Attach( characterToDelete );
                context.Entry( characterToDelete ).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}