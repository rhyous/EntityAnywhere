FROM node:18

WORKDIR /UI

COPY package*.json ./

RUN npm i
RUN npm i --g @angular/cli@13.1.2

COPY . .

ENV serviceUrl='http://localhost:5000/api/'

CMD [ "ng", "serve", "--host", "0.0.0.0" ]
