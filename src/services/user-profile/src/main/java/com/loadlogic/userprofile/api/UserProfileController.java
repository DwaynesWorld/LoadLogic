package com.loadlogic.userprofile.api;

import java.util.List;
import java.util.stream.Collectors;

import com.loadlogic.userprofile.application.CreateUserProfileRequest;
import com.loadlogic.userprofile.application.UserProfileService;
import com.loadlogic.userprofile.domain.UserProfile;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("api/v1/userprofiles")
@Slf4j
public class UserProfileController {

    @Autowired
    private UserProfileService userProfileService;

    @PostMapping("")
    public ResponseEntity<CreateUserProfileResponse> create(@RequestBody CreateUserProfileRequest request) {
        log.info("Creating user profile.");

        var userProfile = userProfileService.createUserProfile(request);
        return new ResponseEntity<>(makeCreateUserProfileResponse(userProfile), HttpStatus.CREATED);
    }

    @GetMapping("")
    public ResponseEntity<GetUserProfilesResponse> getUserProfiles() {
        log.info("Finding all user profile.");

        var profiles = userProfileService
            .findAll()
            .stream()
            .map(p -> makeGetUserProfileResponse(p))
            .collect(Collectors.toList());
        return new ResponseEntity<>(makeGetUserProfilesResponse(profiles), HttpStatus.OK);
    }

    @GetMapping("/{id}")
    public ResponseEntity<GetUserProfileResponse> get(@PathVariable("id") long id) {
        log.info("Finding user profile.");

        return userProfileService
            .findById(id)
            .map(p -> new ResponseEntity<>(makeGetUserProfileResponse(p), HttpStatus.OK))
            .orElseThrow();
    }

    private CreateUserProfileResponse makeCreateUserProfileResponse(UserProfile profile) {
        return new CreateUserProfileResponse(
            profile.getId(), 
            profile.getUserId(), 
            profile.getCompanyId(),
            profile.getDisplayName(), 
            profile.getAvatarUrl());
    }

    private GetUserProfileResponse makeGetUserProfileResponse(UserProfile profile) {
        return new GetUserProfileResponse(
            profile.getId(), 
            profile.getUserId(), 
            profile.getCompanyId(),
            profile.getDisplayName(), 
            profile.getAvatarUrl());
    }

    private GetUserProfilesResponse makeGetUserProfilesResponse(List<GetUserProfileResponse> profiles) {
        return new GetUserProfilesResponse(profiles);
    }
}
