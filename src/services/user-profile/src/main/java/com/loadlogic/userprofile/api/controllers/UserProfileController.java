package com.loadlogic.userprofile.api.controllers;

import java.util.List;

import com.loadlogic.userprofile.domain.UserProfile;
import com.loadlogic.userprofile.domain.UserProfileRepository;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("api/v1/userprofiles")
@Slf4j
public class UserProfileController {
    
    @Autowired
    private UserProfileRepository userProfileRepository;

    @PostMapping("")
    public UserProfile createUserProfile(@RequestBody UserProfile userProfile) {
        log.info("Creating user profile.");

        return userProfileRepository.save(userProfile);
    }

    @GetMapping("")
    public List<UserProfile> findAllUserProfiles() {
        log.info("Finding all user profile.");

        return userProfileRepository.findAll();
    }

    
    @GetMapping("/{id}")
    public UserProfile findUserProfileById(@PathVariable("id") Long id) {
        log.info("Finding user profile.");
        
        return userProfileRepository.findById(id).orElseThrow();
    }

    // Should Combine into query param for findAll.
    @GetMapping("/userId/{userId}")
    public List<UserProfile> findUserProfileByUserId(@PathVariable("userId") Long userId) {
        log.info("Finding user profile.");

        return userProfileRepository.findUserProfileByUserId(userId);
    }
}
