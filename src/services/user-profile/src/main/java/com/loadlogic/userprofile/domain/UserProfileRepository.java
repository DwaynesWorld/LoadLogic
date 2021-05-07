package com.loadlogic.userprofile.domain;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface UserProfileRepository extends JpaRepository<UserProfile, Long> {
    
    List<UserProfile> findUserProfileByUserId(Long userId);
}
