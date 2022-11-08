FROM node:16.18

WORKDIR /app

COPY ./package.json .

RUN ["yarn", "install"]

ENV PATH=/app/node_modules/.bin:$PATH

WORKDIR /app/code