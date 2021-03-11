using System.Collections.Generic;

namespace VintageBookshelf.Api.Dtos
{
    public class UserTokenDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClaimDto> Claims { get; set; }
    }
}