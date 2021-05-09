package com.loadlogic.userprofile;

import com.loadlogic.userprofile.application.UserProfileService;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

@SpringBootApplication
public class UserProfileApplication {
    
    @Bean
    public UserProfileService userProfileService() {
        return new UserProfileService();
    }

	public static void main(String[] args) {
		SpringApplication.run(UserProfileApplication.class, args);
	}
}
