import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {retry} from 'rxjs/operators';
import {environment} from '../../../environments/environment';

/**
 * Course API Service
 */
@Injectable({
  providedIn: 'root'
})
export class CoursesService {

  /**
   *
   * @param http contain http details
   */
  constructor(private http: HttpClient) {

  }

  /**
   * Get all Courses from API
   *
   * @param pageNumber contain pageNumber details
   * @param pageSize contain pageSize details
   */
  getAllCourses(pageNumber, pageSize, keyword,sortType,  sortField) {
    return this.http.get(environment.apiUrl + 'Course/' + pageNumber + '/' + pageSize + '?search='+keyword+'&sortType='+sortType+'&sortField='+sortField).pipe(
      retry(3)
    );
  }

  /**
   * Delete Course API
   *
   * @param id contain id details
   */
  deleteCourse(id) {
    return this.http.delete(environment.apiUrl + 'Course/' + id);
  }
}
