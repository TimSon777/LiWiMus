import {Injectable} from '@nestjs/common';
import {Track} from "./track.entity";
import {TrackDto} from "./dto/track.dto";
import {FilterOptions} from "../filters/filter.options";
import {FilterOptionsService} from "../filters/services/filter.options.service";
import {PlaylistTrack} from "../playlistTracks/playlistTrack.entity";
import {Artist} from "../artists/artist.entity";

@Injectable()
export class TracksService {
    public async trackDtoTest(options: FilterOptions, filterOptionsService: FilterOptionsService) {
        const track = Track.find(
            filterOptionsService.GetFindOptionsObject(options))
        let t = await Artist.find({id: 17});
        
    }
} 
