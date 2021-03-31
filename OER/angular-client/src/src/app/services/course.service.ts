import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../environments/environment';
import {retry} from 'rxjs/operators';


/**
 * Course API Service
 */
@Injectable({
  providedIn: 'root'
})
export class CourseService {
  /**
   *
   * @param http
   */
  constructor(private http: HttpClient) {
  }

  /**
   *get all Courses API
   *
   * @param pageNumber
   * @param pageSize
   */
  getCourses(pageNumber, pageSize): Observable<any> {
    return this.http.get(environment.apiUrl + 'Course/' + pageNumber + '/' + pageSize)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get resource by course id API
   *
   * @param id
   */
  getResourceByCourseId(id): Observable<any> {
    return this.http.get(environment.apiUrl + 'Resource/GetResourceByCourseId/' + id)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *get course test API
   *
   * @param id
   */
  getCourseTest(id): Observable<any> {
    return this.http.get(environment.apiUrl + 'Course/GetCourseTest/' + id)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *create course test API
   *
   * @param data
   */
  postCourseTest(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/SubmitUserAnswers', data);
  }

  /**
   *get course by id API
   *
   * @param slug
   */
  getCourseBySlug(slug): Observable<any> {
    return this.http.get(environment.apiUrl + 'Course/' + slug)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *delete course by id
   *
   * @param slug
   */
  deleteCourseBySlug(slug): Observable<any> {
    return this.http.delete(environment.apiUrl + 'Course/' + slug)
      .pipe(
        retry(3) // retry a failed request up to 3 time
      );
  }

  /**
   *comment on a course API
   *
   * @param data
   */
  commentCourse(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/CommentOnCourse', data);
  }

  /**
   *content withdrawal API
   *
   * @param id
   */
  contentWithdrawal(id): Observable<any> {
    return this.http.put(environment.apiUrl + 'Course/ContentWithdrawal?contentTypeId=1&contentId=' + id, {});
  }

  /**
   *download content of course API
   *
   * @param data
   */
  downloadContentForCourse(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/DownloadContentForCourse', data);
  }

  /**
   *download course content API
   *
   * @param contentId
   */
  downloadContent(contentId): Observable<any> {
    return this.http.get(environment.apiUrl + 'ContentMedia/DownloadResourcesFiles/' + contentId + '/1', {
      headers: new HttpHeaders({
        'Content-Type': 'application/zip',
      }), responseType: 'blob'
    });
  }

  /**
   *get course enrollment status API
   *
   * @param data
   */
  postCourseEnrollment(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/CourseEnrollment', data);
  }

  /**
   *update  course enrollment status API
   *
   * @param data
   */
  postCourseEnrolledStatus(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/CourseEnrolledStatus', data).pipe(
      retry(3)
    );
  }

  /**
   *update course comment API
   *
   * @param data
   */
  updateCommentCourse(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/UpdateCourseComment', data);
  }

  /**
   *delete course comment API
   *
   * @param commentId
   * @param requestedBy
   */
  deleteCourseComment(commentId, requestedBy): Observable<any> {
    return this.http.delete(environment.apiUrl + 'Course/DeleteCourseComment/' + commentId + '/' + requestedBy);
  }

  /**
   *hide course comment API
   *
   * @param commentId
   * @param courseId
   * @param requestedBy
   */
  hideCourseComment(commentId, courseId, requestedBy): Observable<any> {
    return this.http.get(environment.apiUrl + 'Course/HideCourseComment/' + commentId + '/' + courseId + '/' + requestedBy);
  }

  /**
   *report course API
   *
   * @param data
   */
  reportCourse(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/ReportCourse', data);
  }

  /**
   *report course comment API
   *
   * @param data
   */
  reportCourseComment(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/ReportCourseComment', data);
  }

  /**
   *rate course API
   *
   * @param data
   */
  rateCourse(data): Observable<any> {
    return this.http.post(environment.apiUrl + 'Course/RateCourse', data);
  }

  /**
   *create course API
   *
   * @param data
   */
  postCourse(data): Observable<any> {
    const course = data;
    let keywords = '';
    if (data.keywords && data.keywords.length > 0) {
      data.keywords.forEach((item, index) => {
        if (index < (data.keywords.length - 1)) {
          keywords += item.value + ', ';
        } else {
          keywords += item.value;
        }
      });
    } else {
      keywords = '';
    }
    const courseFiles = [];
    if (data.courseFiles && data.courseFiles.length > 0) {
      data.courseFiles.forEach((item) => {
        courseFiles.push({
          associatedFile: item.url,
          fileName: item.fileName
        });
      });
    }
    const sections = [];
    if (course.sections) {
      course.sections.forEach((section) => {
        const courseResources = [];
        if (section.courseResources && section.courseResources.length > 0) {
          section.courseResources.forEach((item) => {
            courseResources.push({
              resourceId: item.id,
              sectionName: section.name
            });
          });
          sections.push({
            name: section.name,
            courseResources: courseResources
          });
        }
      });
    }
    const postData = {
      title: course.title,
      categoryId: +course.categoryId,
      subCategoryId: +course.subCategoryId,
      thumbnail: course.thumbnail,
      courseDescription: course.courseDescription,
      keywords: keywords,
      courseContent: course.courseContent,
      // materialTypeId: +course.materialTypeId,
      copyRightId: course.copyRightId != null ? +course.copyRightId : null,
      educationId: course.educationId != null ? +course.educationId : null,
      professionId: course.professionId != null ? +course.professionId : null,
      readingTime: course.readingTime != null ? +course.readingTime : null,
      isDraft: course.isDraft,
      references: course.references,
      tests: course.tests != null ? [course.tests] : [],
      sections: sections,
      resourceFiles: courseFiles,
      createdBy: course.createdBy,
      emailUrl: course.emailUrl,
      educationalStandardId: course.educationalStandardId != null ? +course.educationalStandardId : null,
      educationalUseId: course.educationalUseId != null ? +course.educationalUseId : null,
      levelId: course.levelId != null ? +course.levelId : null,
      objective: course.objective,
    };
    return this.http.post(environment.apiUrl + 'Course', postData);
  }

  /**
   *Update course API
   *
   * @param data
   * @param slug
   */
  patchCourse(data, slug): Observable<any> {
    const course = data;
    let keywords = '';
    if (data.keywords && data.keywords.length > 0) {
      data.keywords.forEach((item, index) => {
        if (index < (data.keywords.length - 1)) {
          keywords += item.value + ', ';
        } else {
          keywords += item.value;
        }
      });
    } else {
      keywords = '';
    }
    const courseFiles = [];
    if (data.courseFiles && data.courseFiles.length > 0) {
      data.courseFiles.forEach((item) => {
        courseFiles.push({
          associatedFile: item.url,
          fileName: item.fileName
        });
      });
    }
    const sections = [];
    if (course.sections) {
      course.sections.forEach((section) => {
        const courseResources = [];
        if (section.courseResources && section.courseResources.length > 0) {
          section.courseResources.forEach((item) => {
            courseResources.push({
              resourceId: item.id,
              sectionName: section.name
            });
          });
          sections.push({
            name: section.name,
            courseResources: courseResources
          });
        }
      });
    }
    const postData = {
      id: slug,
      title: course.title,
      categoryId: +course.categoryId,
      subCategoryId: +course.subCategoryId,
      thumbnail: course.thumbnail,
      courseDescription: course.courseDescription,
      keywords: keywords,
      courseContent: course.courseContent,
      // materialTypeId: +course.materialTypeId,
      copyRightId: course.copyRightId != null ? +course.copyRightId : null,
      educationId: course.educationId != null ? +course.educationId : null,
      professionId: course.professionId != null ? +course.professionId : null,
      readingTime: course.readingTime != null ? +course.readingTime : null,
      isDraft: course.isDraft,
      references: course.references,
      tests: course.tests != null ? [course.tests] : [],
      sections: sections,
      resourceFiles: courseFiles,
      createdBy: course.createdBy,
      emailUrl: course.emailUrl,
      educationalStandardId: course.educationalStandardId != null ? +course.educationalStandardId : null,
      educationalUseId: course.educationalUseId != null ? +course.educationalUseId : null,
      levelId: course.levelId != null ? +course.levelId : null,
      objective: course.objective
    };
    return this.http.put(environment.apiUrl + 'Course/' + slug, postData);
  }
}
