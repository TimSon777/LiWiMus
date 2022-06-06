import * as dotenv from 'dotenv';
import { resolve } from 'path';
dotenv.config({ path: resolve(__dirname, '../.env') });

import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import {DocumentBuilder, SwaggerModule} from "@nestjs/swagger";

async function bootstrap() {
  const app = await NestFactory.create(AppModule, { cors: true });
  app.setGlobalPrefix('/api');
  const config = new DocumentBuilder()
      .setTitle("LiWiMus.Admin.Web.API.Nest")
      .addServer("http://localhost:3001")
      .addOAuth2({
          type: 'oauth2',
          flows: {
              password: {
                  tokenUrl: process.env.AUTH_URL,
                  scopes: []
              }
          }

      }, 'swagger')
      .build();

  const document = SwaggerModule.createDocument(app, config);
  
  SwaggerModule.setup('swagger', app, document);
  
  await app.listen(3001);
}
bootstrap();