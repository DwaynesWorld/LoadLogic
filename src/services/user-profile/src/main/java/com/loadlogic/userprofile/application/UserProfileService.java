package com.loadlogic.userprofile.application;

import java.util.List;
import java.util.Optional;

import com.loadlogic.userprofile.domain.UserProfile;
import com.loadlogic.userprofile.domain.UserProfileRepository;

import org.springframework.beans.factory.annotation.Autowired;

public class UserProfileService {

    @Autowired
    private UserProfileRepository userProfileRepository;

    public UserProfile createUserProfile(CreateUserProfileRequest request) {
        var profile = new UserProfile(
            request.getUserId(), 
            request.getCompanyId(), 
            request.getDisplayName(),
            request.getAvatarUrl());
        return userProfileRepository.save(profile);
    }

    public List<UserProfile> findAll() {
        return userProfileRepository.findAll();
    }

    public Optional<UserProfile> findById(long id) {
        return userProfileRepository.findById(id);
    }
}
