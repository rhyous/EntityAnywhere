# base image
FROM node

#Set the working directory
WORKDIR /app

#add /app/nodemodules.bon:$PATH
ENV PATH /app/node_modules/.bin:$PATH

# Add app
COPY . /app

# Install and cache app dependencies
COPY package.json /app/package.json
RUN npm install
RUN npm install -g @angular/cli@8.3.18

RUN ng build --configuration=production

FROM nginx:alpine
COPY /dist/* /usr/share/nginx/html/
