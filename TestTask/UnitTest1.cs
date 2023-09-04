using Task.ViewModel.ViewModelsForManagerWindow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace TestTask
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var viewModel = new ForManagerViewModel(); // ������� ��������� ViewModel

            // Act
            MethodInfo methodInfo = typeof(ForManagerViewModel).GetMethod("LoadManagers", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(viewModel, null);


            // Assert
            Assert.IsNotNull(viewModel.Managers); // ���������, ��� ��������� �� �����
            Assert.IsTrue(viewModel.Managers.Count > 0); // ���������, ��� ��������� ��������� �������
        }
    }

}
