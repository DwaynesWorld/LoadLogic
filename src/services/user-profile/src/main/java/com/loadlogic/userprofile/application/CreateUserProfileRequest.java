package com.loadlogic.userprofile.application;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

@NoArgsConstructor
@AllArgsConstructor
@Getter
public class CreateUserProfileRequest {

    private long userId;
    private long companyId;
    private String avatarUrl;
    private String displayName;

}
