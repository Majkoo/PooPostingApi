﻿using AutoMapper;
using PooPosting.Api.Entities;
using PooPosting.Api.Models.Dtos.Account;
using PooPosting.Api.Profilers.ValueResolvers;
using PooPosting.Api.Services.Helpers;
using PooPosting.Api.Models.Dtos;

namespace PooPosting.Api.Profilers;

public class AccountMappingProfile: Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(dto => dto.ProfilePicUrl, opt => opt.MapFrom<PfPicUrlResolver>())
            .ForMember(dto => dto.BackgroundPicUrl, opt => opt.MapFrom<BgPicUrlResolver>())
            .ForMember(dto => dto.IsModifiable, opt => opt.MapFrom<ModifiableResolver>())
            .ForMember(dto => dto.IsAdminModifiable, opt => opt.MapFrom<AdminModifiableResolver>())
            .ForMember(dto => dto.Email, opt => opt.MapFrom(a => a.IsDeleted ? string.Empty : a.Email))
            .ForMember(dto => dto.Nickname, opt => opt.MapFrom(a => a.Nickname))
            .ForMember(dto => dto.Id, opt => opt.MapFrom(a => IdHasher.EncodeAccountId(a.Id)))
            .ForMember(dto => dto.RoleId, opt => opt.MapFrom(acc => acc.Role.Id));

        CreateMap<AccountDto, Account>()
            .ForMember(acc => acc.Id, opt => opt.MapFrom(ato => IdHasher.DecodeAccountId(ato.Id)));

        CreateMap<CreateAccountDto, Account>()
            .ForMember(acc => acc.AccountCreated, opt => opt.MapFrom(c => DateTime.Now));

    }
}