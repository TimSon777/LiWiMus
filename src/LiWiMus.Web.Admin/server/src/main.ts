import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import {DocumentBuilder, SwaggerModule} from "@nestjs/swagger";

async function bootstrap() {
  const app = await NestFactory.create(AppModule, { cors: true });
  app.setGlobalPrefix('/api');
  
  const config = new DocumentBuilder()
      .setTitle("LiWiMus.Admin.Web.API.Nest")
      .build();
  
  const document = SwaggerModule.createDocument(app, config);
  
  SwaggerModule.setup('api', app, document);
  
  await app.listen(3001);
}
bootstrap();