FROM node:16.14.2

WORKDIR /app

COPY . .

RUN cd ./client && npm ci  && npm run build && cd ..

RUN cd ./server && npm ci  && cd ..

RUN mkdir -p /app/server/public

RUN cp -r ./client/build/* ./server/public/

WORKDIR  /app/server

RUN npm run prebuild

RUN npm run build

CMD [ "npm", "run", "start:prod" ]