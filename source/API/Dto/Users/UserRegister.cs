using Mapster;
using SocialWorkApi.Domain.Entities.Users;

namespace SocialWorkApi.API.Dto.Users;

public class UserRegister() : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.NewConfig<User, UserDto>()
        // .IgnoreMember((u, side) => u.Name == "CreatedById" && side == MemberSide.Source)
        // .IgnoreMember((u, side) => u.Name == "LastUpdatedById" && side == MemberSide.Source);
    }
}