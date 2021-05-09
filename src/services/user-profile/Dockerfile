FROM gradle:7.0.0-jdk16 AS build
COPY --chown=gradle:gradle services/user-profile/ /home/gradle/src
WORKDIR /home/gradle/src
RUN gradle build --no-daemon

FROM arm64v8/openjdk:16-jdk-slim
EXPOSE 9001
COPY --from=build /home/gradle/src/build/libs/*.jar app.jar
ENTRYPOINT ["java","-jar","/app.jar"]