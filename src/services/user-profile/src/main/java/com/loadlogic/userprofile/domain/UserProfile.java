package com.loadlogic.userprofile.domain;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Index;
import javax.persistence.Table;

import lombok.Getter;

@Entity
@Table(indexes = { @Index(name = "idx_user_id", columnList = "userId"),
        @Index(name = "idx_company_id", columnList = "companyId"), })
public class UserProfile {

    private static final String UNUSED = "unused";

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Getter
    private long id;

    @Getter
    private long userId;

    @Getter
    private long companyId;

    @Getter
    private String displayName;

    @Getter
    private String avatarUrl;

    @SuppressWarnings(UNUSED)
    private UserProfile() {
    }

    public UserProfile(long userId, long companyId, String displayName, String avatarUrl) {
        this.userId = userId;
        this.companyId = companyId;
        this.displayName = displayName;
        this.avatarUrl = avatarUrl;
    }
}
