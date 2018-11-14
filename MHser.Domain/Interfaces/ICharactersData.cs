using MHser.Domain.Entities;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface ICharactersData
    {
        IEnumerable<Character> GetCharacters();
        Character GetCharacterById( int id );
        Character AddCharacter( Character character );
        void ModifyCharacter( Character character );
        void DeleteCharacter( int id );
    }
}
