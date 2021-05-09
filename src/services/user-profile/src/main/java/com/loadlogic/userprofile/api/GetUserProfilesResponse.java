package com.loadlogic.userprofile.api;

import java.util.List;

import lombok.AllArgsConstructor;
import lombok.Getter;

@AllArgsConstructor
@Getter
public class GetUserProfilesResponse {

    private List<GetUserProfileResponse> userProfiles;
}
