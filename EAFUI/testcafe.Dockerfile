FROM alpine:edge
ARG packageId

COPY testcafe-docker.sh /opt/testcafe/docker/testcafe-docker.sh

RUN apk --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing/ --repository http://dl-cdn.alpinelinux.org/alpine/v3.10/main/ upgrade && \
 apk --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing/ --repository http://dl-cdn.alpinelinux.org/alpine/v3.10/main/ add \
 libevent nodejs npm chromium firefox-esr xwininfo xvfb dbus eudev ttf-freefont fluxbox procps

RUN npm install -g testcafe && \
 chmod +x /opt/testcafe/docker/testcafe-docker.sh && \
 adduser -D user

RUN npm install testcafe testcafe-angular-selectors axios @angular/core rxjs
ENV NODE_PATH=/opt/testcafe/node_modules
ENV PATH=${PATH}:/opt/testcafe/node_modules/.bin

USER user
EXPOSE 1337 1338
ENTRYPOINT ["/opt/testcafe/docker/testcafe-docker.sh"]