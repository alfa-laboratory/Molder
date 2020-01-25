using System.Collections.Generic;

namespace AlfaBank.AFT.Core.Models.Web.Interfaces
{
    public interface IPage
    {
        string Name { get; }
        string Url { get; }
        string AttrUrl { get; }
        string Title { get; }

        /// <summary>
        /// Получение элемента по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IElement GetElementByName(string name);

        /// <summary>
        /// Получение списка элементов, не помеченных аттрибутом @Optional/@Hidden
        /// </summary>
        /// <returns></returns>
        List<IElement> GetPrimaryElements();

        /// <summary>
        /// Получение всех элементов страницы, помеченных аттрибутом @Hidden
        /// </summary>
        /// <returns></returns>
        List<IElement> GetHiddenElements();

        void Close();
        void GoToPage();

        void Refresh();
        void Maximize();
        void PageTop();
        void PageDown();

        /// <summary>
        /// Проверка что все элементы отображаются на странице, за исключением @Hidden которые скрыты и @Optional
        /// </summary>
        /// <returns></returns>
        bool IsAppeared();

        /// <summary>
        /// Проверка, что все элементы без аттрибута @Optional исчезли
        /// </summary>
        /// <returns></returns>
        bool IsDisappeared();

        void IsPageLoad();
    }
}
