import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ServeStaticModule } from '@nestjs/serve-static';
import { join } from 'path';
import {UsersModule} from "./users/users.module";
import { ArtistsController } from './artists/artists.controller';
import { ArtistsModule } from './artists/artists.module';
import { TracksModule } from './tracks/tracks.module';
import { TransactionsModule } from './transactions/transactions.module';
import { GenresModule } from './genres/genres.module';
import { AlbumsModule } from './albums/albums.module';
import { PlaylistsModule } from './playlists/playlists.module';
import {AuthModule} from "./auth/auth.module";
import {ConfigModule} from "@nestjs/config";
import {APP_GUARD} from "@nestjs/core";
import {AdminAuthGuard} from "./auth/admin.auth.guard";

@Module({
  imports: [
    TypeOrmModule.forRoot(),
    ServeStaticModule.forRoot({
      rootPath: join(__dirname, '..', 'public'),
      serveRoot: '/admin',
    }),
    ConfigModule.forRoot(),
    UsersModule,
    ArtistsModule,
    TracksModule,
    TransactionsModule,
    GenresModule,
    AlbumsModule,
    PlaylistsModule,
    AuthModule,
  ],
  controllers: [AppController],
  providers: [
      AppService,
    {
      provide: APP_GUARD,
      useClass: AdminAuthGuard,
    }
  ],
})
export class AppModule {}
