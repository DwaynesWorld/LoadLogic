package com.loadlogic.userprofile.api;

import lombok.AllArgsConstructor;
import lombok.Getter;

@AllArgsConstructor
@Getter
public class GetUserProfileResponse {

    private long id;
    private long userId;
    private long companyId;
    private String displayName;
    private String avatarUrl;
    
}
