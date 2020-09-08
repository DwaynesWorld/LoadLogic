FROM node:latest as build

WORKDIR /app
COPY . ./
RUN yarn install
RUN yarn build

FROM nginx:alpine

COPY --from=build /app/build /usr/share/nginx/html
# To provide a http authentication comment out the next two lines
#COPY conf/default.conf /etc/nginx/conf.d/default.conf
#COPY conf/authnginx/htpasswd /etc/nginx/authnginx/htpasswd
EXPOSE 80 2222
CMD ["nginx", "-g", "daemon off;"]